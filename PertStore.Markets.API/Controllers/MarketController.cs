using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MarketController : ControllerBase
    {
        private readonly IMarketService _marketService;
        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }
        [HttpGet("Market/{MarketName}")]
        public async Task<ActionResult<BaseResult<MarketDto>>> GetMarketAsync(string MarketName)
        {
            var result = await _marketService.GetMarketAsync(MarketName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("MarketGuid/{marketGuidId}")]
        public async Task<ActionResult<BaseResult<MarketDto>>> GetMarketGuidAsync(string marketGuidId)
        {
            var result = await _marketService.GetMarketGuidAsync(marketGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("ProductInMarket/{MarketName}/{productName}")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> GetProductInMarketAsync(string productName,string MarketName)
        {
            var result = await _marketService.GetProductInMarketAsync(productName,MarketName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("ProductInMarketGuid/{productName}/{MarketGuidId}")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> GetProductInMarketGuidAsync(string productName, string MarketGuidId)
        {
            var result = await _marketService.GetProductInMarketGuidAsync(productName, MarketGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("ProductGuidInMarket/{MarketName}/{productGuid}")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> GetProductGuidInMarketAsync(string productGuid, string MarketName)
        {
            var result = await _marketService.GetProductGuidInMarketAsync(productGuid,MarketName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("ProductGuidInMarketGuid/{MarketGuidId}/{productGuidId}")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> GetProductGuidInMarketGuidAsync(string productGuidId, string MarketGuidId)
        {
            var result = await _marketService.GetProductGuidInMarketGuidAsync(productGuidId, MarketGuidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("PlusProductInMarket")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> PlusProductInMarketAsync(MarketProductLineDto dto)
        {
            var result = await _marketService.PlusProductInMarketAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("CreateMarket")]
        public async Task<ActionResult<BaseResult<MarketDto>>> CreateMarketAsync(CreateMarketDto dto)
        {
            var result = await _marketService.CreateMarketAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("ProductInMarket")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> AddProductInMarket(MarketProductLineDto dto)
        {
            var result = await _marketService.AddProductInMarket(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<ActionResult<BaseResult<MarketDto>>> UpdateMarketAsync(MarketUpdateDto dto)
        {
            var result = await _marketService.UpdateMarketAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("ProductInMarket/{MarketGuid}/{NameProduct}")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> RemoveProductInMarketAsync(string MarketGuid,string NameProduct)
        {
            var result = await _marketService.RemoveProductInMarketAsync(MarketGuid, NameProduct);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Market/{guidId}")]
        public async Task<ActionResult<BaseResult<MarketDto>>> DeleteMarketAsync(string guidId)
        {
            var result = await _marketService.DeleteMarketAsync(guidId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("MinusProductInMarket/{NameProduct}/{MarketGuid}")]
        public async Task<ActionResult<BaseResult<ProductLineDto>>> MinusProductInMarketAsync(string MarketGuid,string NameProduct)
        {
            var result = await _marketService.MinusProductInMarketAsync(MarketGuid,NameProduct);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
