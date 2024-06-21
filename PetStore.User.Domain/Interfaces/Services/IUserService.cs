using PetStore.Users.Domain.Dto;
using PetStore.Users.Domain.Dto.User;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// метод считывания пользователя из БД
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> GetUserAsync(string userLogin);
        /// <summary>
        /// метод регистрации нового пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> RegistrationUserAsync(RegistrationUserDto dto);
        /// <summary>
        /// метод входа пользователя в акаунт
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> LoginUserAsync(LoginUserDto dto);
        /// <summary>
        /// метод обновления данных пользователя
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto userDto);
        /// <summary>
        /// метод удаления пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> DeleteUserAsync(string Email,string Passport);
        /// <summary>
        /// метод для считывания всех пользователей из БД
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<UserDto>> GetAllUsersAsync();
        /// <summary>
        /// метод считывания Id пользователя из бд
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<BaseResult<UserGuidDto>> GetUserGuidIdAsync(string userLogin, string password);
    }
}
