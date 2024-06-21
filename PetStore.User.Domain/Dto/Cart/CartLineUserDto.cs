using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.Cart
{
    public record CartLineUserDto(string userLogin, string GuidId);
}
