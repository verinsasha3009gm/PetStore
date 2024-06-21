using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;

namespace PetStore.Users.API.Controllers
{
    /// <summary>
    /// контроллер роли
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        /// <summary>
        /// создание роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> CreateRoleAsync(RoleDto dto)
        {
            var result = await _roleService.CreateRoleAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> UpdateRoleAsync(UpdateRoleDto dto)
        {
            var result = await _roleService.UpdateRoleAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> DeleteRoleAsync(string RoleName)
        {
            var result = await _roleService.DeleteRoleAsync(RoleName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Считывание роли из БД
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> GetRoleAsync(string Role)
        {
            var result = await _roleService.GetRoleAsync(Role);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
