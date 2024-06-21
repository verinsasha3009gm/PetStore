using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using PetStore.Markets.Domain.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Service
{
    public class TokenService : ITokenService
    {
        private readonly IBaseRepository<Employe> _employeeRepository;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options, IBaseRepository<Employe> employeeRepo)
        {
            _jwtKey = options.Value.JwtKey;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _employeeRepository = employeeRepo;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken =
                new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentails);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                ValidateLifetime = true,
                ValidAudience = _audience,
                ValidIssuer = _issuer,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase))
                throw new SecurityTokenException(/*ErrorMessage.InvalidToken*/);
            return claimsPrincipal;
        }

        public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
        {
            string accessToken = dto.AccessToken;
            string refreshToken = dto.RefreshToken;

            var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
            var userName = claimsPrincipal.Identity?.Name;

            var user = await _employeeRepository.GetAll().Include(p => p.Token).FirstOrDefaultAsync(p => p.Name == userName);

            if (user == null)
            {
                if (user.Token.RefreshToken != refreshToken)
                {
                    if (user.Token.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    {
                        return new BaseResult<TokenDto>
                        {
                            //ErrorMessage = ErrorMessage.InvalidClientRequrest,
                            //ErrorCode = (int)ErrorCodes.InvalidClientRequest
                        };
                    }
                }
            }

            var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            if (user.Token == null)
            {
                var token = new Token()
                {
                    RefreshToken = newRefreshToken,
                };
                user.Token = token;
                _employeeRepository.UpdateAsync(user);
                await _employeeRepository.SaveChangesAsync();
                return new BaseResult<TokenDto>()
                {
                    Data = new TokenDto()
                    {
                        AccessToken = accessToken,
                        RefreshToken = newRefreshToken,
                    }
                };
            }
            user.Token.RefreshToken = newRefreshToken;
            _employeeRepository.UpdateAsync(user);
            await _employeeRepository.SaveChangesAsync();
            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                }
            };
        }
    }
}
