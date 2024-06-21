using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Address
{
    public record AddressUserDto(string Country, string Region, string City, string Street, string Login, string Password);
}
