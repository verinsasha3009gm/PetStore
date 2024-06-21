using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.User
{
    public record UserGuidDto(string GuidId,string Login, string Email);
}
