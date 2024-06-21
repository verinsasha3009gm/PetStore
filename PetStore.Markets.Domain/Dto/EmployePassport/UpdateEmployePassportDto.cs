using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.EmployePassport
{
    public record UpdateEmployePassportDto(string Post, decimal Expirience, long Salary, string Email,string Password);
}
