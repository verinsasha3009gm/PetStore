using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Dto.Employe;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IEmployeService
    {
        /// <summary>
        /// метод считывания пользователя из БД
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<EmployeDto>> GetEmployeAsync(string Email);
        /// <summary>
        /// метод регистрации нового пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<EmployeDto>> RegistrationEmployeAsync(RegistrationEmployeDto dto);
        /// <summary>
        /// метод входа пользователя в акаунт
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> LoginEmployeAsync(LoginEmployeDto dto);
        /// <summary>
        /// метод обновления данных пользователя
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<BaseResult<EmployeDto>> UpdateEmployeAsync(UpdateEmployeDto userDto);
        /// <summary>
        /// метод удаления пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<EmployeDto>> DeleteEmployeAsync(string Email, string Password);
        /// <summary>
        /// метод для считывания всех пользователей из БД
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<EmployeDto>> GetAllEmployesAsync();
        /// <summary>
        /// метод считывания Id пользователя из бд
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<BaseResult<EmployeGuidDto>> GetEmployeGuidIdAsync(string Email, string password);
    }
}
