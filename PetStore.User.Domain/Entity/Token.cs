using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class Token : IEntityId<long>
    {
        public long Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
