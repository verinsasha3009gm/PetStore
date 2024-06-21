using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Services
{
    public interface IUserRoleService
    {
        /// <summary>
        /// Добавление роли для пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto);
        /// <summary>
        /// Удаление роли у пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(string userLogin, string RoleName);
        /// <summary>
        /// Обновление роли у пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto);
    }
}
