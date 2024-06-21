using PetStore.Markets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Entity
{
    public class ProductLine : IEntityId<long>
    {
        public long Id { get; set; }
        public int Count { get; set; }
        public string GuidId { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }
        [JsonIgnore]
        public List<Market>? Markets { get; set; }
        [JsonIgnore]
        public List<MarketCapital>? MarketCapitals { get; set; }
    }
}
