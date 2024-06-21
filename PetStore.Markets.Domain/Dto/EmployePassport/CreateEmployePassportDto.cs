using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.EmployePassport
{
    public record CreateEmployePassportDto(string Post, decimal Expirience, long Salary,string Email);
}
