using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using PetStore.Markets.Application.Mapping;
using PetStore.Markets.Application.Service;
using PetStore.Markets.DAL;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using PetStore.Markets.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetStore.Markets.Consumer
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _chanel;
        private readonly IOptions<RabbitMqSettings> _options;
        private readonly IProductService _productService;
        public RabbitMqListener(IOptions<RabbitMqSettings> options)
        {
            _options = options;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.QueueDeclare(_options.Value.QueueName, durable: true, exclusive: false, autoDelete: false,
                arguments: null);

            var optionsDb = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.MarketData;User Id=postgres;Password=qwerpoiu")
                .Options;
            var DbContext = new ApplicationDbContext(optionsDb);
            var repositoryMockProduct = new BaseRepository<Product>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            _productService = new ProductService(repositoryMockProduct, mapper, logger.Object, cache);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("-----");
            var rand = new Random();
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += async (obj, basicDeliver) =>
            {
                var context = Encoding.UTF8.GetString(basicDeliver.Body.ToArray());
                Debug.WriteLine($"Получено сообщение: {context}");
                try
                {
                    var body = JsonConvert.DeserializeObject<MessageResult<Object>>(context);
                    var HttpMethod = body.HttpMethod;
                    var resultType = body.TypeMessage;
                    switch (resultType)
                    {
                        case "Product":
                            {
                                var bodyProd = JsonConvert.DeserializeObject<MessageResult<Product>>(context);
                                var prod = bodyProd.Data;
                                switch (HttpMethod)
                                {
                                    case "POST":
                                        {
                                            await _productService.CreateProductInRabbit(prod);
                                            Console.WriteLine("Create Product");
                                            break;

                                        }
                                    case "PUT":
                                        {
                                            await _productService.UpdateProductInRabbit(prod);
                                            Console.WriteLine("Update Product");
                                            break;
                                        }
                                    case "DELETE":
                                        {
                                            await _productService.DeleteProductInRabbit(prod.GuidId);
                                            Console.WriteLine("Delete Product");
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
                catch (Exception ex) { }
                _chanel.BasicAck(basicDeliver.DeliveryTag, false);
            };

            _chanel.BasicConsume(_options.Value.QueueName, false, consumer);
            Dispose();
            return Task.CompletedTask;
        }
    }
}
