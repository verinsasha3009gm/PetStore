using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Result
{
    public class MessageResult<T> : MessageResult
    {
        public T Data;
    }
    public class MessageResult
    {
        public string HttpMethod;
        public string TypeMessage;
    }
}
