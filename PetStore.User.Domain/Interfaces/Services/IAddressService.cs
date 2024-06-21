using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Services
{
    public interface IAddressService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<BaseResult<Address>> AddAddressInRabbit(Address address);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        Task<BaseResult<Address>> RemoveAddressInRabbit(string guidId);
        /// <summary>
        /// метод получения адреса у пользователя
        /// </summary>
        /// <param name="guidId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<AddressDto>> GetAddressInUserAsync(string guidId, string userLogin);
        /// <summary>
        /// метод добавления адреса в список адресов пользователя
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<BaseResult<AddressDto>> AddAddressInUserAsync(AddressInUserDto address);
        /// <summary>
        /// метод удаления адреса в списке адреса пользователя
        /// </summary>
        /// <param name="guidId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<AddressDto>> RemoveAddressInUserAsync(string guidId, string userLogin);
        /// <summary>
        /// метод считывания всех адрессов у пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<CollectionResult<AddressDto>> GetAllAddressesInUserAsync(string userLogin);
    }
}
