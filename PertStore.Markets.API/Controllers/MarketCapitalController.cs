using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Dto.MarketCapital;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MarketCapitalController : ControllerBase
    {
        private readonly IMarketCapitalService _marketCapitalService;
        public MarketCapitalController(IMarketCapitalService marketCapitalService)
        {
            _marketCapitalService = marketCapitalService;
        }
        [HttpGet("MarketCapital/{Day}/{guidMarketId}")]
        public async Task<ActionResult<BaseResult<MarketCapitalDto>>> GetMarketCapitalAsync(string Day,string guidMarketId)
        {
            var result = await _marketCapitalService.GetMarketCapitalAsync(Day,guidMarketId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("ProdLinesInMarket/{guidMarketId}")]
        public async Task<ActionResult<CollectionResult<ProductLineDto>>> GetProductsLinesGuidAsync(string guidMarketId)
        {
            var result = await _marketCapitalService.GetProductsLinesGuidAsync(guidMarketId);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Add")]
        public async Task<ActionResult<BaseResult<MarketCapitalDto>>> AddProductLineInMarketAsync(MarketCapitalProductLineDto dto)
        {
            var result = await _marketCapitalService.AddProductLineInMarketAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Plus")]
        public async Task<ActionResult<BaseResult<MarketCapitalDto>>> PlusProductLineInMarketAsync(ProductLineNameDto dto)
        {
            var result = await _marketCapitalService.PlusProductLineInMarketAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Minus/{MarketGuidId}/{Day}/{NameProduct}")]
        public async Task<ActionResult<BaseResult<MarketCapitalDto>>> MinusProductLineInMarketAsync(string MarketGuidId, string Day, string NameProduct)
        {
            var result = await _marketCapitalService.MinusProductLineInMarketAsync(MarketGuidId,Day,NameProduct);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Delete/{MarketGuidId}/{Day}/{NameProduct}")]
        public async Task<ActionResult<BaseResult<MarketCapitalDto>>> RemoveProductLineInMarketAsync(string MarketGuidId, string Day, string NameProduct)
        {
            var result = await _marketCapitalService.RemoveProductLineInMarketAsync(MarketGuidId,Day,NameProduct);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
