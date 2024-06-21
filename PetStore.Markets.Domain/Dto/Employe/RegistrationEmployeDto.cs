using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Employe
{
    public record RegistrationEmployeDto(string Name, string Gender,string Email, string Password, string AlreadyPassword, string Post, decimal Expirience, long Salary);
}
