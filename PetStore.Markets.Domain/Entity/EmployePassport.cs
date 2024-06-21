using PetStore.Markets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Entity
{
    public class EmployePassport : IEntityId<long>
    {
        public long Id { get; set; }
        public string GuidId { get; set; }
        public long Salary {  get; set; }
        public string Post {  get; set; }
        public decimal Experience { get; set; }
        [JsonIgnore]
        public Address? Address { get; set; }
        public long? AddressId { get; set; }
        [JsonIgnore]
        public Employe? Employe { get; set; }
        public long? EnployeId { get; set; }
    }
}
