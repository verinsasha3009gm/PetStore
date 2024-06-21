using PetStore.Markets.Domain.Interfaces;

namespace PetStore.Markets.Domain.Entity
{
    public class MarketCapital : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public long DailyIncome { get; set; }
        public List<ProductLine> ProductsSold { get; set; }
        public DateTime Date {  get; set; }
        public Market? Market { get; set; }
        public long? MarketId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
