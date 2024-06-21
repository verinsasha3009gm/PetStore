using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Dto.Role
{
    public record UpdateUserRoleDto(string UserLogin, string ThenRole, string NewRole);
}
