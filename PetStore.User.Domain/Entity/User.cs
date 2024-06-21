using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class User : IEntityId<long>
    {
        public long Id {  get; set; }
        public string GuidId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }
        public long CartId { get; set; }
        [JsonIgnore]
        public List<Address> Addresses { get; set; }
        [JsonIgnore]
        public Token Token { get; set; }
        public long TokenId { get; set; }
        [JsonIgnore]
        public List<Role> Roles { get; set; }
    }
}
