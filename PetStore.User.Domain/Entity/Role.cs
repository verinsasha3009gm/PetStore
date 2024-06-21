using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class Role : IEntityId<long>
    {
        public long Id { get;set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
