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
    public class Address :IEntityId<long>
    {
        public long Id { get; set; }
        public string GuidId { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public long? UserId { get; set; }
        public long? MarketId { get; set; }
        public long? EmployePassportId { get; set; }
    }
}
