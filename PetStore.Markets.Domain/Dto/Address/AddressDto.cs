using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Address
{
    public record AddressDto(string Country, string Region, string City, string Street);
}
