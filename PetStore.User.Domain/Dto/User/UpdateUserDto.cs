using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.User
{
    public record UpdateUserDto(string LastEmeil,string LastPassport, string Login, string NewEmail, string NewPassport);
}
