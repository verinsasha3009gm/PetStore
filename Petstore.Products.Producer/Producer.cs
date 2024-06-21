using Newtonsoft.Json;
using Petstore.Products.Producer.Interfaces;
using RabbitMQ.Client;
using PetStore.Products.Domain.Result;
using System.Text;

namespace PetStore.Products.Producer
{
    public class Producer : IMessageProducer
    {
        public void SendMessage<T>(T message,string httpMethod, string routingKey, string? exchange = null)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var p = nameof(message);
            var messResult = new MessageResult<T>()
            {
                Data = message,
                HttpMethod = httpMethod,
                TypeMessage = message.GetType().Name,
            };
            var json = JsonConvert.SerializeObject(messResult, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange, routingKey, body: body);
            Console.WriteLine( "-----------11");
        }
    }
}
