using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Settings
{
    public class RedisSettings
    {
        public string Url { get; set; }
        public string InstanceName { get; set; }
    }
}
