using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PetStore.Products.Domain.Interfaces;

namespace PetStore.Products.Domain.Entity
{
    public class Product : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string GuidId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Description> DescriptionList { get; set; }

        public List<Teg> Tegs { get; set; }
        [JsonIgnore]
        public ProductPassport? ProductPassport { get; set; }
        public long? ProductPassportId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
