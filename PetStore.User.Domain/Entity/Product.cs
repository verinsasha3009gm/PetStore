using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class Product : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string GuidId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long ProductPassportId { get; set; }
        public long CategoryId { get; set; }
        public List<CartLine> CartLines { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
