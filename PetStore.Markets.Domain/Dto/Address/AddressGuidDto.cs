using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Address
{
    public record AddressGuidDto(string GuidId, string Country, string Region, string City, string Street);
}
