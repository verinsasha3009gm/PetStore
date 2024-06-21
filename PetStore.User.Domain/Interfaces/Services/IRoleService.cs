using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Добавление роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> CreateRoleAsync(RoleDto dto);
        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> DeleteRoleAsync(string name);
        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> UpdateRoleAsync(UpdateRoleDto dto);
        /// <summary>
        /// считывание роли
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> GetRoleAsync(string name);
    }
}
