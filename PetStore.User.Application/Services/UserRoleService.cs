using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PetStore.Users.Domain.Enum;
using PetStore.Users.Application.Resources;
using Serilog;

namespace PetStore.Users.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        public UserRoleService( IBaseRepository<User> UserRepository, IBaseRepository<Role> RoleRepository,
            IBaseRepository<UserRole> userRoleRepository, ILogger logger,IUnitOfWork unitOfWork)
        {
            _userRepository = UserRepository;
            _roleRepository = RoleRepository;
            _userRoleRepository = userRoleRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
        {
            var user = await _userRepository.GetAll().Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Login == dto.UserLogin);
            if (user == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound,
                };
            }

            var roles = user.Roles.Select(p => p.Name).ToArray();
            if (roles.FirstOrDefault(p => p == dto.RoleName) == null)
            {
                var role = await _roleRepository.GetAll()
                    .FirstOrDefaultAsync(p => p.Name == dto.RoleName);
                if (role == null)
                {
                    return new BaseResult<UserRoleDto>()
                    {
                        ErrorCode = (int)ErrorCodes.RoleNotFound,
                        ErrorMessage = ErrorMessage.RoleNotFound
                    };
                }
                var userRole = new UserRole()
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };
                try
                {
                    await _userRoleRepository.CreateAsync(userRole);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<UserRoleDto>()
                    {
                        ErrorCode = (int)ErrorCodes.InternalServerException,
                        ErrorMessage = ErrorMessage.InternalServerException
                    };
                }
                return new BaseResult<UserRoleDto>()
                {
                    Data = new UserRoleDto(user.Login,role.Name)
                };
            }
            return new BaseResult<UserRoleDto>()
            {
                ErrorCode = (int)ErrorCodes.RoleAlreadyExists,
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
            };
        }

        public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(string userLogin, string RoleName)
        {
            var user = await _userRepository.GetAll().Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Login == userLogin);
            if (user == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                };
            }
            var role = user.Roles.FirstOrDefault(p => p.Name == RoleName);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.RoleNotFound,
                    ErrorMessage =ErrorMessage.RoleNotFound,
                };
            }
            var userRole = _userRoleRepository.GetAll().Where(p => p.RoleId == role.Id)
                .FirstOrDefault(p => p.UserId == user.Id);
            if(userRole == null)
            {
                return new()
                {
                    ErrorMessage = ErrorMessage.UserRoleNotFound,
                    ErrorCode = (int)ErrorCodes.UserRoleNotFound
                };
            }
            try
            {
                _userRoleRepository.DeleteAsync(userRole);
                await _userRoleRepository.SaveChangesAsync();
                user.Roles.Remove(role);
                _userRepository.UpdateAsync(user);
                await _userRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<UserRoleDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto(user.Login,role.Name)
            };
        }

        public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
        {
            var user = await _userRepository.GetAll().Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Login == dto.UserLogin);
            if (user == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound,
                };
            }
            var role = user.Roles.FirstOrDefault(p => p.Name == dto.ThenRole);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }
            var roleNew = user.Roles.FirstOrDefault(p => p.Name == dto.NewRole);
            if (roleNew != null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleAlreadyExists,
                    ErrorCode = (int)ErrorCodes.RoleAlreadyExists
                };
            }
            var newRoleForUser = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.NewRole);
            if (newRoleForUser == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound
                };
            }
            var userRole = await _userRoleRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefaultAsync(p => p.RoleId == role.Id);
            if(userRole == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserRoleNotFound,
                    ErrorMessage = ErrorMessage.UserRoleNotFound
                };
            }
            var newUserRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = newRoleForUser.Id,
            };
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _userRoleRepository.DeleteAsync(userRole);
                    await _userRoleRepository.SaveChangesAsync();
                    await _userRoleRepository.CreateAsync(newUserRole);
                    await transaction.CommitAsync();
                    return new BaseResult<UserRoleDto>()
                    {
                        Data = new UserRoleDto(dto.UserLogin,dto.NewRole)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex,ex.Message);
                    return new BaseResult<UserRoleDto>
                    {
                        ErrorCode = (int)ErrorCodes.InternalServerException,
                        ErrorMessage = ErrorMessage.InternalServerException
                    };
                };
            }
        }
    }
}
