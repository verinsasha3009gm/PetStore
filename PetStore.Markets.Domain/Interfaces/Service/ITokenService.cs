using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface ITokenService
    {
        /// <summary>
        /// метод генерации Ацесс токена
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);
        /// <summary>
        /// метод генерации рефреш токена
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
        /// <summary>
        ///  метод создания валидного токена 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        /// <summary>
        /// метод создания валидного токена для API
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);
    }
}
