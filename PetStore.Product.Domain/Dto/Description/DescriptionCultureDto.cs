using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Dto.Description
{
    public record DescriptionCultureDto(string prodName,string culture, string detail,List<string> productСomposition);
}
