using PetStore.Products.Domain.Interfaces;

namespace PetStore.Products.Domain.Entity
{
    public class Teg : IEntityId<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
