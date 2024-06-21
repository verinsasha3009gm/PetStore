using PetStore.Markets.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace PetStore.Markets.Domain.Entity
{
    public class Market :IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GuidId { get; set; }
        [JsonIgnore]
        public Address? Adress { get; set; }
        public long? AdressId { get; set; }
        [JsonIgnore]
        public List<ProductLine> RangeProducts { get; set; }
        [JsonIgnore]
        public List<Employe> Employes { get; set; }
        [JsonIgnore]
        public List<MarketCapital> MarketCapitals { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
