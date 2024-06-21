using PetStore.Markets.Domain.Dto.Address;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IAddressService
    {
        Task<BaseResult<AddressDto>> GetAddressAsync(string guidId);
        Task<BaseResult<AddressGuidDto>> CreateAddressAsync(AddressDto dto);
        Task<BaseResult<AddressDto>> UpdateAddressAsync(AddressGuidDto dto);
        Task<BaseResult<AddressDto>> DeleteAddressAsync(string guidId);
        //User
        Task<CollectionResult<AddressGuidDto>> GetAddressesGuidInUserAsync(string guidId);
        Task<CollectionResult<AddressDto>> GetAddressesInUserAsync(string Login , string Password);
        Task<BaseResult<AddressGuidDto>> AddAddressInUserAsync(AddressUserDto dto);
        Task<BaseResult<AddressGuidDto>> AddAddressInUserGuidAsync(AddressUserGuidDto dto);
        Task<BaseResult<AddressDto>> RemoveAddressInUserAsync(string Login,string Password, string addressGuidId);
        Task<BaseResult<AddressDto>> RemoveAddressInUserGuidAsync(string userGuidId,string Passworrd,string addressGuidId);
        Task<BaseResult<AddressGuidDto>> GetUserGuidAddressAsync(string userIdGuid, string addressGuidId);
        Task<BaseResult<AddressGuidDto>> GetUserAddressAsync(string Login, string addressGuidId);
        //Market
        Task<BaseResult<AddressDto>> AddAddressInMarketAsync(AddressMarketDto dto);
        Task<BaseResult<AddressDto>> AddAddressInMarketGuidAsync(AddressMarketGuidDto dto);
        Task<BaseResult<AddressDto>> RemoveAddressInMarketAsync(string Name);
        Task<BaseResult<AddressDto>> RemoveAddressInMarketGuidAsync(string MarketGuidId);
        Task<BaseResult<AddressGuidDto>> GetMarketGuidAddressAsync(string MarketGuidId);
        Task<BaseResult<AddressGuidDto>> GetMarketAddressAsync(string MarketName);
        //Employe
        Task<BaseResult<AddressDto>> AddAddressInEmployePassportAsync(AddressEmployePassportDto dto);
        Task<BaseResult<AddressDto>> AddAddressInEmployePassportGuidAsync(AddressEmployePassportGuidDto dto);
        Task<BaseResult<AddressDto>> RemoveAddressInEmployePassportAsync(string Name);
        Task<BaseResult<AddressDto>> RemoveAddressInEmployePassportGuidAsync(string EmployePassportGuidId);
        Task<BaseResult<AddressGuidDto>> GetEmployePassportGuidAddressAsync(string EmployePassportGuidId);
        Task<BaseResult<AddressGuidDto>> GetEmployePassportAddressAsync(string EmployeName);
    }
}
