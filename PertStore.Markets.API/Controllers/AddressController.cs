using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto.Address;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet("Get/{addressGuidId}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> GetAddressAsync(string addressGuidId)
        {
            var result = await _addressService.GetAddressAsync(addressGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Get/Employe/{EmployeName}")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> GetEmployePassportAddressAsync(string EmployeName)
        {
            var result = await _addressService.GetEmployePassportAddressAsync(EmployeName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Get/EmployeGuid/{EmployePassportGuidId}")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> GetEmployePassportGuidAddressAsync(string EmployePassportGuidId)
        {
            var result = await _addressService.GetEmployePassportGuidAddressAsync(EmployePassportGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("Get/Market/{MarketName}")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> GetMarketAddressAsync(string MarketName)
        {
            var result = await _addressService.GetMarketAddressAsync(MarketName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Get/MarketGuid/{MarketGuidId}")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> GetMarketGuidAddressAsync(string MarketGuidId)
        {
            var result = await _addressService.GetMarketGuidAddressAsync(MarketGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Get/User/{Login}/{Password}")]
        public async Task<ActionResult<CollectionResult<AddressDto>>> GetAddressesInUserAsync(string Login,string Password)
        {
            var result = await _addressService.GetAddressesInUserAsync(Login,Password);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("Get/UserGuid/{guidId}")]
        public async Task<ActionResult<CollectionResult<AddressGuidDto>>> GetAddressesGuidInUserAsync(string guidId)
        {
            var result = await _addressService.GetAddressesGuidInUserAsync(guidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Get/UserGuid/{UserGuidId}/{addressGuidId}")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> GetAddressGuidInUserAsync(string UserGuidId,string addressGuidId)
        {
            var result = await _addressService.GetUserGuidAddressAsync(UserGuidId, addressGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Get/UserAddress/{Login}/{addressGuidId}")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> GetUserAddressAsync(string Login, string addressGuidId)
        {
            var result = await _addressService.GetUserAddressAsync(Login, addressGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("Create")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> CreateAddressAsync(AddressDto dto)
        {
            var result = await _addressService.CreateAddressAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Create/EmpPassport")]
        public async Task<ActionResult<BaseResult<AddressDto>>> AddAddressInEmployePassportAsync(AddressEmployePassportDto dto)
        {
            var result = await _addressService.AddAddressInEmployePassportAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Create/EmpPassportGuid")]
        public async Task<ActionResult<BaseResult<AddressDto>>> AddAddressInEmployePassportGuidAsync(AddressEmployePassportGuidDto dto)
        {
            var result = await _addressService.AddAddressInEmployePassportGuidAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Create/Market")]
        public async Task<ActionResult<BaseResult<AddressDto>>> AddAddressInMarketAsync(AddressMarketDto dto)
        {
            var result = await _addressService.AddAddressInMarketAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Create/MarketGuid")]
        public async Task<ActionResult<BaseResult<AddressDto>>> AddAddressInMarketGuidAsync(AddressMarketGuidDto dto)
        {
            var result = await _addressService.AddAddressInMarketGuidAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Create/User")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> AddAddressInUserAsync(AddressUserDto dto)
        {
            var result = await _addressService.AddAddressInUserAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Create/UserGuid")]
        public async Task<ActionResult<BaseResult<AddressGuidDto>>> AddAddressInUserGuidAsync(AddressUserGuidDto dto)
        {
            var result = await _addressService.AddAddressInUserGuidAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("Update")]
        public async Task<ActionResult<BaseResult<AddressDto>>> UpdateAddressAsync(AddressGuidDto dto)
        {
            var result = await _addressService.UpdateAddressAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("Delete/{GuidId}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> DeleteAddressAsync(string GuidId)
        {
            var result = await _addressService.DeleteAddressAsync(GuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Remove/EmployePassport/{Name}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInEmployePassportAsync(string Name)
        {
            var result = await _addressService.RemoveAddressInEmployePassportAsync(Name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Remove/EmployePassportGuid/{EmployePassportGuidId}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInEmployePassportGuidAsync(string EmployePassportGuidId)
        {
            var result = await _addressService.RemoveAddressInEmployePassportGuidAsync(EmployePassportGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Remove/Market/{Name}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInMarketAsync(string Name)
        {
            var result = await _addressService.RemoveAddressInMarketAsync(Name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Remove/MarketGuid/{MarketGuidId}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInMarketGuidAsync(string MarketGuidId)
        {
            var result = await _addressService.RemoveAddressInMarketGuidAsync(MarketGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Remove/User/{Login}/{Password}/{addressGuid}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInUserAsync(string Login,string Password,string addressGuid)
        {
            var result = await _addressService.RemoveAddressInUserAsync(Login, Password, addressGuid);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Remove/UserGuid/{userGuidId}/{Password}/{addressGuid}")]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInUserGuidAsync(string userGuidId, string Password, string addressGuid)
        {
            var result = await _addressService.RemoveAddressInUserGuidAsync(userGuidId, Password, addressGuid);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
    }
}
