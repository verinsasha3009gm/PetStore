using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Entity
{
    public class Token
    {
        public long Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        [JsonIgnore]
        public Employe Employe { get; set; }
        public long EmployeId { get; set; }

        public long UserId { get; set; }
    }
}
