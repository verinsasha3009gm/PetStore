using PetStore.Markets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Entity
{
    public class User : IEntityId<long>
    {
        public long Id {  get; set; }
        public string GuidId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public List<Address> Adresses { get; set; }
        public long CartId { get; set; }
        public long TokenId { get; set; }
    }
}
