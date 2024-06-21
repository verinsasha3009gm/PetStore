using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Users.Domain.Dto.Cart;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;

namespace PetStore.Users.API.Controllers
{
    /// <summary>
    /// контроллер корзины
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        /// <summary>
        /// считывание корзины у пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpGet("{userLogin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CartDto>>> GetUserCartAsync(string userLogin)
        {
            var result = await _cartService.GetUserCartAsync(userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// добавление продука в корзину пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("/Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CartLineDto>>> AddUserCartLinesAsync(CartLineUserDto dto)
        {
            var result = await _cartService.AddUserCartLineAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Очистка корзины у пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpDelete("Clear/{userLogin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CartDto>>> ClearUserCartAsync(string userLogin)
        {
            var result = await _cartService.ClearUserCartAsync(userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// удаление продукта у пользователя
        /// </summary>
        /// <param name="guidId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpDelete("Remove/{userLogin}/{guidId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CartLineDto>>> RemoveUserCartLineAsync(string guidId, string userLogin)
        {
            var result = await _cartService.RemoveUserCartLineAsync(guidId,userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// считывание всех продуктов с корзины пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpGet("UserAllCartLines/{userLogin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<CartLineDto>>> GetUserAllCartLinesAsync(string userLogin)
        {
            var result = await _cartService.GetUserAllCartLinesAsync(userLogin);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
