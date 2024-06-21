using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Passport
{
    public record UpdatePassportDto(string PlaceOfBirth, string Issued,string Name, string Familien, long PassportNumber, string PassportSeria);
}
