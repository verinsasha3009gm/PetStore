using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;

namespace PetStore.Users.API.Controllers
{
    /// <summary>
    /// контроллер адреса
    /// </summary>
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
        /// <summary>
        /// считывание адреса у пользователя
        /// </summary>
        /// <param name="guidId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpGet("{guidId}/{userLogin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<AddressDto>>> GetAddressInUserAsync(string guidId, string userLogin)
        {
            var result = await _addressService.GetAddressInUserAsync(guidId, userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// считывание всех адресов у пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpGet("{userLogin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<AddressDto>>> GetAllAddressesInUserAsync(string userLogin)
        {
            var result = await _addressService.GetAllAddressesInUserAsync(userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// добавление адреса у пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<AddressDto>>> AddAddressInUserAsync(AddressInUserDto dto)
        {
            var result = await _addressService.AddAddressInUserAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// удаление адреса у пользователя
        /// </summary>
        /// <param name="guidId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpDelete("{guidId}/{userLogin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<AddressDto>>> RemoveAddressInUserAsync(string guidId, string userLogin)
        {
            var result = await _addressService.RemoveAddressInUserAsync(guidId, userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
