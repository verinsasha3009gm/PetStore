using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PetStore.Products.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Consumer
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _chanel;
        private readonly IOptions<RabbitMqSettings> _options;
        public RabbitMqListener(IOptions<RabbitMqSettings> options)
        {
            _options = options;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.QueueDeclare(_options.Value.QueueName, durable: true, exclusive: false, autoDelete: false,
                arguments: null);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rand = new Random();
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += async (obj, basicDeliver) =>
            {
                var context = Encoding.UTF8.GetString(basicDeliver.Body.ToArray());
                Debug.WriteLine($"Получено сообщение: {context}");
                _chanel.BasicAck(basicDeliver.DeliveryTag, false);
            };

            _chanel.BasicConsume(_options.Value.QueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
