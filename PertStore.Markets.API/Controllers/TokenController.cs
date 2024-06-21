using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
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
        [HttpPost]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshTokenAsync(TokenDto dto)
        {
            var result = await _tokenService.RefreshToken(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
