using PetStore.Library.BaseInterfaces;

namespace PetStore.Library.Entity.Product
{
    public class ProductPasport : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string Company { get; set; }

        public Product Product { get; set; }
        public long ProductId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}