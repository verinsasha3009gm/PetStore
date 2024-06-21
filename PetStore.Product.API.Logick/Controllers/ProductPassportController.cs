using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Products.Domain.Dto.ProductPassport;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/")]
    public class ProductPassportController : Controller
    {
        private readonly IProductPassportService _productPassportService;
        public ProductPassportController(IProductPassportService productPassportService)
        {
            _productPassportService = productPassportService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Company"></param>
        /// <param name="prodPassportName"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> GetProductPassportAsync(string Company, string prodPassportName)
        {
            var result = await _productPassportService.GetProductPassportAsync(Company, prodPassportName);
            if(result.IsSucces)
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
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> CreateProductPassportAsync(ProductPassportDto dto)
        {
            var result = await _productPassportService.CreateProductPassportAsync(dto);
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
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> UpdateProductPassportAsync(UpdateProductPassportDto dto)
        {
            var result = await _productPassportService.UpdateProductPassportAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Company"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> DeleteProductPassportAsync(string Name, string Company)
        {
            var result = await _productPassportService.DeleteProductPassportAsync(Name,Company);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameProduct"></param>
        /// <param name="nameProductPassport"></param>
        /// <param name="companyProductPassport"></param>
        /// <returns></returns>
        [HttpGet("GetProductPassportInProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductPassportDto>>>
            GetPassportInProductAsync(string nameProduct, string nameProductPassport, string companyProductPassport)
        {
            var result = await _productPassportService.GetPassportInProductAsync(nameProduct, nameProductPassport, companyProductPassport);
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
        [HttpPost("AddProductPassportInProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> AddPassportInProductAsync(ProductInProductPassportDto dto)
        {
            var result = await _productPassportService.AddPassportInProductAsync(dto);
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
        [HttpPut("UpdateProductPassportInProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> UpdatePassportInProductAsync(ProductInProductPassportDto dto)
        {
            var result = await _productPassportService.UpdatePassportInProductAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameProduct"></param>
        /// <param name="nameProductPassport"></param>
        /// <param name="companyProductPassport"></param>
        /// <returns></returns>
        [HttpDelete("RemoveProductPassportInProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ProductPassportDto>>> RemovePassportInProductAsync(string nameProduct, string nameProductPassport, string companyProductPassport)
        {
            var result = await _productPassportService.RemovePassportInProductAsync(nameProduct,nameProductPassport,companyProductPassport);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
