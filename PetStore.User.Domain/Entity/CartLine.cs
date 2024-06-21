using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class CartLine : IEntityId<long>
    {
        public long Id { get; set; }
        public int Count { get; set; }

        public Product Product { get; set; }
        public long ProductId { get; set; }

        public Cart Cart { get; set; }
        public long CartId { get; set; }
    }
}
