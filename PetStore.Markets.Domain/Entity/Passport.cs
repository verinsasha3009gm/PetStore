using PetStore.Markets.Domain.Interfaces;

namespace PetStore.Markets.Domain.Entity
{
    public class Passport : IEntityId<long>
    {
        public long Id {  get; set; }
        public string Name { get; set; }
        public string Familien {  get; set; }
        public string DepartmentCode {  get; set; }
        public string PassportSeria { get; set; }
        public string Issued {  get; set; }
        public string PlaceOfBirth { get; set; }
        public long PassportNumber {  get; set; }
        public DateTime PassportDateIssue { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Employe Employe { get; set; }
        public long PassportId { get; set;}
    }
}