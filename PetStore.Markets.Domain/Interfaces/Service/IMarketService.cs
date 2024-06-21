using PetStore.Markets.Domain.Dto.Address;
using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IMarketService
    {
        Task<BaseResult<MarketDto>> GetMarketGuidAsync(string guidId);
        Task<BaseResult<MarketDto>> GetMarketAsync(string MarketName);
        Task<BaseResult<MarketDto>> CreateMarketAsync(CreateMarketDto dto);
        Task<BaseResult<MarketDto>> UpdateMarketAsync(MarketUpdateDto dto);
        Task<BaseResult<MarketDto>> DeleteMarketAsync(string guidId);
        //Cart

        Task<BaseResult<ProductLineDto>> GetProductInMarketAsync(string productName, string MarketName);
        Task<BaseResult<ProductLineDto>> GetProductInMarketGuidAsync(string productName, string MarketGuid);
        Task<BaseResult<ProductLineDto>> GetProductGuidInMarketAsync(string productGuid, string MarketName);
        Task<BaseResult<ProductLineDto>> GetProductGuidInMarketGuidAsync(string productGuid,string MarketGuid);

        Task<BaseResult<ProductLineDto>> AddProductInMarket(MarketProductLineDto dto);
        Task<BaseResult<ProductLineDto>> PlusProductInMarketAsync(MarketProductLineDto dto);
        Task<BaseResult<ProductLineDto>> RemoveProductInMarketAsync(string MarketGuid, string NameProduct);
        Task<BaseResult<ProductLineDto>> MinusProductInMarketAsync(string MarketGuid, string NameProduct);
    }
}
