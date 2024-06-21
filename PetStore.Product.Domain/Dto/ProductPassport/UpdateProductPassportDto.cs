using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Dto.ProductPassport
{
    public record UpdateProductPassportDto(string LastName, string LastCompany, string newName,string newDescription,string newCompany);
}
