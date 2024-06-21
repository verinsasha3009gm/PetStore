using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.Address
{
    public record AddressInUserDto(string userLogin, string Region, string Country, string City, string Street);
}
