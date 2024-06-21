using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Product
{
    public record ProductDto(string Name ,string Description,long Price);
}
