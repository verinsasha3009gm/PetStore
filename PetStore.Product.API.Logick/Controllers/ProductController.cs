using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Products.Domain.Dto;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Controllers
{
    /// <summary>
    /// Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]/")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductGuidDto>>> GetProductAsync(string name)
        {
            var result = await _productService.GetProductAsync(name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpGet("ProductCategoryAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<ProductDto>>> GetProductCategoryAllAsync(string categoryName)
        {
            var result = await _productService.GetAllProductInCategory(categoryName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProductAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<ProductDto>>> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> CreateProductDtoAsync(CreateProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductDto>>> UpdateProductDtoAsync(UpdateProductDto dto)
        {
            var result = await _productService.UpdateProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("Delete")]
        public async Task<ActionResult<BaseResult<ProductDto>>> DeleteProductDtoAsync(string name)
        {
            var result = await _productService.DeleteProductAsync(name);
            if(result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
