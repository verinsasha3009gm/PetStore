using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.User
{
    public record UpdateUserDto(string UserGuid, string UserPassword, string NewLogin, string NewEmail);
}
