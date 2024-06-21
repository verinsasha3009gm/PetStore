using PetStore.Products.Domain.Interfaces;

namespace PetStore.Products.Domain.Entity
{
    public class ProductPassport : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GuidId { get; set; }

        public Product? Product { get; set; }
        public long? ProductId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}