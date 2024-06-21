using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Application.Validate;
using PetStore.Markets.Domain.Dto.User;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Enum;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using Serilog;

namespace PetStore.Markets.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _UserRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public UserService(IBaseRepository<User> userRepository, IMapper mapper,ILogger logger, ICacheService cacheService)
        {
            _UserRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == dto.Email);
            if (user != null)
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.UserAlreadyExists,
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                };
            }
            if(dto.AlreadyPassword != dto.Password)
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
                };
            }
            try
            {
                user = new User()
                {
                    Email = dto.Email,
                    GuidId = Guid.NewGuid().ToString(),
                    Password = PasswordValidate.HashPassword(dto.Password),
                    Role = "User",
                    Login = dto.Login,
                };
                await _UserRepository.CreateAsync(user);
                _cacheService.Set(user.GuidId, user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.UserCreateError,
                    ErrorMessage = ErrorMessage.UserCreateError,
                };
            }
            return new BaseResult<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        public async Task<BaseResult<UserDto>> DeleteUserAsync(string guidId, string Password)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
            if (user == null)
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            if (!PasswordValidate.IsVerifyPassword(user.Password, Password))
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
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
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.UserDeleteError,
                    ErrorMessage = ErrorMessage.UserDeleteError,
                };
            }
            return new BaseResult<UserDto>()
            {
                Data = _mapper.Map<UserDto>(user),
            };
        }
        public async Task<BaseResult<UserGuidDto>> GetUserAsync(string Email)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == Email);
            if(user == null)
            {
                return new BaseResult<UserGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            _cacheService.Refrech<User>(user.GuidId);
            return new BaseResult<UserGuidDto>
            {
                Data = new UserGuidDto(user.GuidId.ToString(),user.Login,user.Email,user.Role)
            };
        }
        public async Task<BaseResult<UserDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.userGuidId);
            if (user == null)
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            if (!PasswordValidate.IsVerifyPassword(user.Password, dto.Password))
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
                };
            }
            try
            {
                user.Role = dto.newRole;
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                _cacheService.Set(user.GuidId, user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.UserUpdateError,
                    ErrorMessage = ErrorMessage.UserUpdateError,
                };
            }
            return new BaseResult<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        public async Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto dto)
        {
            var  user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.UserGuid);
            if (user == null)
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            if (!PasswordValidate.IsVerifyPassword(user.Password, dto.UserPassword))
            {
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
                };
            }
            try
            {
                user.Email = dto.NewEmail;
                user.Login = dto.NewLogin;
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                _cacheService.Set(user.GuidId,user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>
                {
                    ErrorCode = (int)ErrorCodes.UserUpdateError,
                    ErrorMessage = ErrorMessage.UserUpdateError,
                };
            }
            return new BaseResult<UserDto>
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
    }
}
