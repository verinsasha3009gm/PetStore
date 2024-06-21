using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Dto.ProductTeg
{
    public record UpdateProductTegDto(string prodName,string fromTeg,string newTeg);
}
