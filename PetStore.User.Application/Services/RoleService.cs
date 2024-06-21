using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PetStore.Users.Domain.Enum;
using PetStore.Users.Application.Resources;
using Serilog;

namespace PetStore.Users.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RoleService(IBaseRepository<Role> RoleRepository,IMapper mapper,ILogger logger)
        {
            _roleRepository = RoleRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseResult<RoleDto>> CreateRoleAsync(RoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.Name);
            if (role != null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleAlreadyExists,
                    ErrorCode = (int)ErrorCodes.RoleAlreadyExists
                };
            }
            role = new Role()
            {
                Name = dto.Name,
            };
            try
            {
                await _roleRepository.CreateAsync(role);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<RoleDto>()
                {
                    ErrorCode= (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role),
            };
        }
        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(string name)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == name);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,
                };
            }
            try
            {
                _roleRepository.DeleteAsync(role);
                await _roleRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<RoleDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }
        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(UpdateRoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.Name);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound,
                };
            }
            try
            {
                role.Name = dto.NewName;
                var updateRole = _roleRepository.UpdateAsync(role);
                await _roleRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new BaseResult<RoleDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role),
            };
        }
        public async Task<BaseResult<RoleDto>> GetRoleAsync(string name)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Name == name);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorCode = (int)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,
                };
            }
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role),
            };
        }
    }
}
