using PetStore.Markets.Domain.Dto.User;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IUserService
    {
        Task<BaseResult<UserGuidDto>> GetUserAsync(string Email);
        Task<BaseResult<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto user);
        Task<BaseResult<UserDto>> UpdateRoleForUserAsync(UpdateUserRoleDto user);
        Task<BaseResult<UserDto>> DeleteUserAsync(string guidId, string Password);
    }
}
