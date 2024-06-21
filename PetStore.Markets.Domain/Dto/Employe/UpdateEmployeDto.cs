using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Employe
{
    public record UpdateEmployeDto(string NewNameEmploye, string NewGender,string Email,string Password);
}
