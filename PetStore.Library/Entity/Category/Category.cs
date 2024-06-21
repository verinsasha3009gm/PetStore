using PetStore.Library.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.Entity.Category
{
    public class Category : IEntityId<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Product.Product> Product { get; set; }
    }
}
