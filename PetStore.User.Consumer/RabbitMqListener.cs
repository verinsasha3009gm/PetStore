using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using PetStore.Users.Application.Mapping;
using PetStore.Users.Application.Services;
using PetStore.Users.DAL;
using PetStore.Users.DAL.Repository;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace PetStore.Users.Consumer
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _chanel;
        private readonly IOptions<RabbitMqSettings> _options;
        private readonly IProductService _productService;
        private readonly IAddressService _addressService;
        public RabbitMqListener(IOptions<RabbitMqSettings> options)
        {
            _options = options;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.QueueDeclare(_options.Value.QueueName, durable: true, exclusive: false, autoDelete: false,
                arguments: null);

            var optionsDb = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.UserData;User Id=postgres;Password=qwertyuiop")
                .Options;
            var DbContext = new ApplicationDbContext(optionsDb);
            var productRepository = new BaseRepository<Product>(DbContext);
            var userRepository = new BaseRepository<User>(DbContext);
            var userRoleRepository = new BaseRepository<UserRole>(DbContext);
            var roleRepository = new BaseRepository<Role>(DbContext);
            var addressRepository = new BaseRepository<Address>(DbContext);
            var cartRepository = new BaseRepository<Cart>(DbContext);
            var cartLineRepository = new BaseRepository<CartLine>(DbContext);
            var unitOfWork = new UnitOfWork(DbContext,productRepository, userRepository, userRoleRepository
                , roleRepository, addressRepository, cartRepository, cartLineRepository);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMapping());
                mc.AddProfile(new AddressMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            _productService = new ProductService(productRepository, mapper, logger.Object, cache);

            _addressService = new AddressService(addressRepository,userRepository,mapper,logger.Object,cache,unitOfWork);
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
                            switch(HttpMethod)
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
                                    break ;
                                }
                                case "DELETE":
                                {
                                    await _productService.DeleteProductInRabbit(prod.GuidId);
                                    Console.WriteLine("Delete Product");
                                    break ;
                                }
                            }
                            break;
                        }
                        case "Address": 
                        {
                            var bodyAddress = JsonConvert.DeserializeObject<MessageResult<Address>>(context);
                            var address = bodyAddress.Data;
                            switch (HttpMethod)
                            {
                                case"POST":
                                {
                                    await _addressService.AddAddressInRabbit(address);
                                    Console.WriteLine("Create Address");
                                    break;
                                }
                                case "DELETE":
                                {
                                    await _addressService.RemoveAddressInRabbit(address.GuidId);
                                    Console.WriteLine("Delete Address");
                                    break ;
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
            return  Task.CompletedTask;
        }
        public override void Dispose()
        {
            //_chanel.Dispose();
            base.Dispose();
        }
    }
}
