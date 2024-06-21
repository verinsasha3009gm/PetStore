using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto.User;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResult<UserGuidDto>>> GetUserAsync(string Email)
        {
            var result = await _userService.GetUserAsync(Email);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<BaseResult<UserDto>>> CreateUserAsync(CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("Role")]
        public async Task<ActionResult<BaseResult<UserDto>>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
        {
            var result = await _userService.UpdateRoleForUserAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult<BaseResult<UserDto>>> UpdateUserAsync(UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<BaseResult<UserDto>>> DeleteUserAsync(string guidId,string Password)
        {
            var result = await _userService.DeleteUserAsync(guidId, Password);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
