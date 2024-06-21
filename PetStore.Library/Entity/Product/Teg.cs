using PetStore.Library.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.Entity.Product
{
    public class Teg : IEntityId<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
