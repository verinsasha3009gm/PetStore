using PetStore.Markets.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace PetStore.Markets.Domain.Entity
{
    public class Product : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string GuidId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }

        public long? ProductPassportId { get; set; }
        public long? CategoryId { get; set; }
        [JsonIgnore]
        public List<ProductLine>? ProductLines { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
