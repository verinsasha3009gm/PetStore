using Newtonsoft.Json;
using PetStore.Markets.Producer.Interfaces;
using PetStore.Markets.Domain.Result;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Producer
{
    public class Producer : IMessageProducer
    {
        public void SendMessage<T>(T message,string HttpMethod, string routingKey, string? exchange = null)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var messageResult = new MessageResult<T>()
            {
                Data = message,
                HttpMethod = HttpMethod,
                TypeMessage = message.GetType().Name,
            };
            var json = JsonConvert.SerializeObject(messageResult, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange, routingKey, body: body);
        }
    }
}
