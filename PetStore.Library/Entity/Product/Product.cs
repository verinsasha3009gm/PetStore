using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetStore.Library.BaseInterfaces;

namespace PetStore.Library.Entity.Product
{
    public class Product : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Description> DescriptionList { get; set; }
        public ProductPasport ProductPasport { get; set; }
        public Category.Category Category { get; set; }
        public List<Teg> Tegs { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
