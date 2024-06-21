using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Users.Domain.Dto;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;

namespace PetStore.Users.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        /// <summary>
        /// получение прав авторизованного пользователя
        /// </summary>
        /// <param name="tokenDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
        {
            var response = await _tokenService.RefreshToken(tokenDto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
