using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Dto.User;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetStore.Users.Domain.Dto;
using System.Security.Cryptography;
using PetStore.Users.Domain.Enum;
using System.Security.Claims;
using AutoMapper;
using PetStore.Users.Application.Resources;
using Serilog;

namespace PetStore.Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _UserRepository;
        private readonly IBaseRepository<Role> _RoleRepository;
        private readonly IBaseRepository<UserRole> _UserRoleRepository;
        private readonly IBaseRepository<Token> _TokenRepository;
        private readonly ITokenService _TokenService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public UserService(IBaseRepository<User> UserRepository, IBaseRepository<Role> roleRepository,
            IBaseRepository<UserRole> userRoleRepository, IBaseRepository<Token> tokenRepository
            , ITokenService tokenService,IMapper mapper,ILogger logger, ICacheService cacheService)
        {
            _UserRepository = UserRepository;
            _RoleRepository = roleRepository;
            _UserRoleRepository = userRoleRepository;
            _TokenRepository = tokenRepository;
            _TokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<UserDto>> DeleteUserAsync(string Email,string Password)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == Email);
            if(user == null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage =ErrorMessage.UserNotFound,
                };
            }
            if(!IsVerifyPassword(user.Password, Password))
            {
                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.PassportNotValid,
                    ErrorMessage = ErrorMessage.PassportNotValid,
                };
            }
            try
            {
                _UserRepository.DeleteAsync(user);
                await _UserRepository.SaveChangesAsync();

                //_cacheService.Delete<User>(user.GuidId);

            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<UserDto>()
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        public async Task<CollectionResult<UserDto>> GetAllUsersAsync()
        {
            var users = await _UserRepository.GetAll().Select(p=> new UserDto(p.Login,p.Email)).ToListAsync();
            if (users ==null)
            {
                return new CollectionResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound,
                };
            }
            return new CollectionResult<UserDto>
            {
                Data = users,
                Count = users.Count()
            };
        }
        public async Task<BaseResult<UserDto>> GetUserAsync(string userLogin)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.Login == userLogin);
            if (user == null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            _cacheService.Refrech<User>(user.GuidId);
            return new BaseResult<UserDto>
            {
                Data = _mapper.Map<UserDto>(user),
            };
        }
        public async Task<BaseResult<UserDto>> RegistrationUserAsync(RegistrationUserDto dto)
        {
            if (dto.Password != dto.AlreadyPassword)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordNotMatch,
                    ErrorCode = (int)ErrorCodes.PasswordNotMatch
                };
            }

            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.Login == dto.Login);
            if (user != null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                    ErrorCode = (int)ErrorCodes.UserAlreadyExists
                };
            }
            var hashUserPassword = HashPassword(dto.Password);
            user = new User()
            {
                Login = dto.Login,
                Email = dto.Email,
                Password = hashUserPassword,
                GuidId = Guid.NewGuid().ToString(),
            };
            try
            {
                await _UserRepository.CreateAsync(user);
                _cacheService.Set(user.GuidId, user);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }

            var role = await _RoleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == nameof(Roles.User));
            if (role == null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound
                };
            }
            UserRole userRole = new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id,
            };
            try
            {
                await _UserRoleRepository.CreateAsync(userRole);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<UserDto>()
            {
                Data = _mapper.Map<UserDto>(user),
            };
        }
        public async Task<BaseResult<TokenDto>> LoginUserAsync(LoginUserDto dto)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (user == null)
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }
            if (!IsVerifyPassword(user.Password, dto.Password))
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.PassportNotValid,
                    ErrorCode = (int)ErrorCodes.PassportNotValid
                };
            }
            var userToken = await _TokenRepository.GetAll().FirstOrDefaultAsync(p => p.UserId == user.Id);

            var userRoles = user.Roles;
            if(userRoles == null)
            {
                user.Roles = new List<Role>();
                var role = await _RoleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == nameof(Roles.User));
                user.Roles.Add(role);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
            }
            userRoles = user.Roles;
            var claims = userRoles.Select(p => new Claim(ClaimTypes.Role, p.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));

            var accessToken = _TokenService.GenerateAccessToken(claims);
            var refreshToken = _TokenService.GenerateRefreshToken();
            if (userToken == null)
            {
                userToken = new Token()
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                };
                await _TokenRepository.CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                _TokenRepository.UpdateAsync(userToken);
                await _TokenRepository.SaveChangesAsync();
            }
            _cacheService.Refrech<User>(user.GuidId);
            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    RefreshToken = refreshToken,
                    AccessToken = accessToken
                }
            };
        }
        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).ToLower();
        }
        private bool IsVerifyPassword(string userPasswordHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return hash == userPasswordHash;
        }
        public async Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto userDto)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == userDto.LastEmeil);
            if (user == null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound,
                };
            }
            if(!IsVerifyPassword(user.Password,userDto.LastPassport))
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordNotMatch,
                    ErrorMessage = ErrorMessage.PasswordNotMatch
                };
            }
            var newUser = await _UserRepository.GetAll().FirstOrDefaultAsync(p=> p.Email == userDto.NewEmail);
            if (newUser != null)
            {
                return new()
                {
                    ErrorCode = (int)ErrorCodes.UserAlreadyExists,
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                };
            }
            try
            {
                user.Email = userDto.NewEmail;
                user.Login = userDto.Login;
                user.Password = HashPassword(userDto.NewPassport);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();

                _cacheService.Set<User>(user.GuidId, user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        public async Task<BaseResult<UserGuidDto>> GetUserGuidIdAsync(string userLogin, string password)
        {
            var user = await _UserRepository.GetAll()
                .FirstOrDefaultAsync(p => p.Login.Equals(userLogin));
            if (user == null)
            {
                return new BaseResult<UserGuidDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var  hachPassword = HashPassword(password);
            if(user.Password != hachPassword)
            {
                return new BaseResult<UserGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportNotValid,
                    ErrorMessage = ErrorMessage.PassportNotValid
                };
            }
            _cacheService.Refrech<User>(user.GuidId);
            return new BaseResult<UserGuidDto>
            {
                Data = new(user.GuidId,user.Login,user.Email)
            };
        }
    }
}
