using PetStore.Markets.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace PetStore.Markets.Domain.Entity
{
    public class Employe : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string GuidId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public Market? Market { get; set; }
        public long? MarketId { get; set; }
        [JsonIgnore]
        public EmployePassport? EmployePassport { get; set; }
        public long? EmployePassportId { get; set; }
        [JsonIgnore]
        public Passport? Passport { get; set; }
        public long? PassportId { get; set;}
        [JsonIgnore]
        public Token? Token { get; set; }
        public long? TokenId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}