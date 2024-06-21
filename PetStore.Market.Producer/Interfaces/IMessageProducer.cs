using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Producer.Interfaces
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(T message,string httpMethod, string routingKey, string? exchange = default);
    }
}
