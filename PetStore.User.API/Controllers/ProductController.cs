using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using System;
using System.Xml.Linq;

namespace PetStore.Users.API.Controllers
{
    /// <summary>
    /// контроллер продукта
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// считывание продукта по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> GetProductAsync(string name)
        {
            var result = await _productService.GetProductAsync(name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Считывание всех продуктов из бд
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<ProductDto>>> GetAllProductAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Удаление продукта по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> DeleteProductAsync(string name)
        {
            var result = await _productService.DeleteProductAsync(name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Создание продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> CreateProductAsync(CreateProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> UpdateProductAsync(UpdateProductDto dto)
        {
            var result = await _productService.UpdateProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Считывание продукта по айди
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        [HttpGet("GuidId/{guidId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> GetProductGuidIdAsync(string guidId)
        {
            var result = await _productService.GetProductGuidIdAsync(guidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// получение айди продукта по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Guid/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductGuidDto>>> GetProductGuidAsync(string name)
        {
            var result = await _productService.GetProductGuidAsync(name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// удаление продукта по айди
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        [HttpDelete("GuiudId/{guidId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> DeleteProductGuidIdAsync(string guidId)
        {
            var result = await _productService.DeleteProductGuidIdAsync(guidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
