using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductLineController : ControllerBase
    {
        private readonly IProductLineService _productLineService;
        public ProductLineController(IProductLineService productLineService)
        {
            _productLineService = productLineService;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> GetProductLineAsync(string GuidId)
        {
            var result = await _productLineService.GetProductLineAsync(GuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> MinusProductLineAsynAsync(string GuidId)
        {
            var result = await _productLineService.MinusProductLineAsync(GuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> PlusProductLineAsync(ProductLineGuidDto dto)
        {
            var result = await _productLineService.PlusProductLineAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
