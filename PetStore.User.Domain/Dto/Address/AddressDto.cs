using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.Address
{
    public record AddressDto( string GuidId, string Country, string Region, string City, string Street);
}
