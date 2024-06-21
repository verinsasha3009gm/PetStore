using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class Cart : IEntityId<long>
    {
        public long Id { get; set; }
        public int Count { get; set; }
        public List<CartLine> Lines { get; set; }
        
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
