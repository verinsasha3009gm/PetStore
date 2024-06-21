using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.Product
{
    public record ProductGuidDto(string GuidId,string ProdName,string Description,decimal Price);
}
