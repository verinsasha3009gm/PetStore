using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.Product
{
    public record CreateProductDto(string Name, string Description, string CategoryName,decimal Price);
}
