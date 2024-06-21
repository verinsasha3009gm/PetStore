using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto.Product;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
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
        [HttpGet]
        public async Task<ActionResult<BaseResult<ProductDto>>> GetProductAsync(string ProductGuidId)
        {
            var result = await _productService.GetProductAsync(ProductGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<BaseResult<ProductGuidDto>>> CreateProductAsync(ProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult<BaseResult<ProductDto>>> UpdateProductAsync(ProductGuidDto dto)
        {
            var result = await _productService.UpdateProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<BaseResult<ProductDto>>> DeleteProductAsync(string ProductGuidId)
        {
            var result = await _productService.DeleteProductAsync(ProductGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
