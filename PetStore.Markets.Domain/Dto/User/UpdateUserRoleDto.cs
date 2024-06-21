using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.User
{
    public record UpdateUserRoleDto(string newRole, string userGuidId, string Password);
}
