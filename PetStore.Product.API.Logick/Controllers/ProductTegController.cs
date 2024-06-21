using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.ProductTeg;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/")]
    public class ProductTegController : Controller
    {
        private readonly IProductTegService _productTegService;
        public ProductTegController(IProductTegService productTegService)
        {
            _productTegService = productTegService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productTegDto"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductTegDto>>> CreateProductTegAsync(ProductTegDto productTegDto)
        {
            var result = await _productTegService.CreateProductTeg(productTegDto);
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
        public async Task<ActionResult<BaseResult<ProductTegDto>>> UpdateProductTegAsync(UpdateProductTegDto dto)
        {
            var result = await _productTegService.UpdateProductTeg(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodName"></param>
        /// <param name="teg"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductTegDto>>> RemoveProductTegAsync(string prodName, string teg)
        {
            var result = await _productTegService.DeleteProductTeg(prodName, teg);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
