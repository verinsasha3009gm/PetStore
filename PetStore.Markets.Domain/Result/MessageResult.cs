using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Result
{
    public class MessageResult
    {
        public string HttpMethod;
        public string TypeMessage;
    }
    public class MessageResult<T> : MessageResult
    {
        public T Data;
    }
}
