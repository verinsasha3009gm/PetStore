using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.User
{
    public record RegistrationUserDto(string Login, string Email, string Password, string AlreadyPassword);
}
