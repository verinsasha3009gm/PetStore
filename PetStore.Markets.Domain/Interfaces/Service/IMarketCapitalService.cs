using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Dto.MarketCapital;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IMarketCapitalService
    {
        Task<BaseResult<MarketCapitalDto>> GetMarketCapitalAsync(string Day, string guidMarketId);
        Task<BaseResult<MarketCapitalDto>> AddProductLineInMarketAsync(MarketCapitalProductLineDto dto);
        Task<BaseResult<MarketCapitalDto>> RemoveProductLineInMarketAsync(string MarketId, string Day,string NameProduct);
        Task<BaseResult<MarketCapitalDto>> PlusProductLineInMarketAsync(ProductLineNameDto dto);
        Task<BaseResult<MarketCapitalDto>> MinusProductLineInMarketAsync(string MarketId, string Day, string NameProduct);
        Task<CollectionResult<ProductLineDto>> GetProductsLinesGuidAsync(string guidMerketId);
    }
}
