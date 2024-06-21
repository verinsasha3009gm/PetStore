using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Dto.MarketCapital;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Enum;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Service
{
    public class MarketCapitalService : IMarketCapitalService
    {
        private readonly IBaseRepository<MarketCapital> _marketCapitalRepository;
        private readonly IBaseRepository<ProductLine> _productLineRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Market> _marketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public MarketCapitalService(IBaseRepository<MarketCapital> marketCapitalRepository,
            IBaseRepository<ProductLine> productLineRepository, IBaseRepository<Product> productRepository,
            IBaseRepository<Market> marketRepository,IMapper mapper,ILogger logger,ICacheService cacheService)
        {
            _marketCapitalRepository = marketCapitalRepository;
            _productLineRepository = productLineRepository;
            _productRepository = productRepository;
            _marketRepository = marketRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<MarketCapitalDto>> AddProductLineInMarketAsync(MarketCapitalProductLineDto dto)
        {
            var market = await _marketRepository.GetAll()
                .Include(p=>p.MarketCapitals)
                .ThenInclude(p=>p.ProductsSold)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p=>p.GuidId == dto.guidId);
            if (market == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage =ErrorMessage.MarketNotFound
                };
            }
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.NameProduct);
            if (prod == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage =ErrorMessage.ProductNotFound
                };
            }
            var markCapt = market.MarketCapitals.FirstOrDefault(p=>p.Date.Date.ToString() == dto.Day);
            var marketCapitalDaily = new MarketCapital()
            {
                ProductsSold = new List<ProductLine>
                {},
                DailyIncome = dto.Count * prod.Price,
                Date = DateTime.UtcNow,
                MarketId = market.Id,
            };
            if (markCapt == null)
            {
               markCapt = await _marketCapitalRepository.CreateAsync(marketCapitalDaily);
            }
            var prodLines = markCapt.ProductsSold;
            var prodLine = prodLines.FirstOrDefault(p => p.Product.Name == dto.NameProduct);
            if (prodLine != null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineAlreadyExists,
                    ErrorMessage = ErrorMessage.ProductLineAlreadyExists
                };
            }
            
            prodLine = new ProductLine()
            {
                ProductId = prod.Id,
                Count = (int)dto.Count,
                GuidId = Guid.NewGuid().ToString(),
                MarketCapitals = new List<MarketCapital>()
            };
            try
            {
                prodLine.MarketCapitals.Add(marketCapitalDaily);
                prodLine = await _productLineRepository.CreateAsync(prodLine);
                _cacheService.Set(prodLine.GuidId, prodLine);
                marketCapitalDaily.ProductsSold.Add(prodLine);

                _marketCapitalRepository.UpdateAsync(marketCapitalDaily);
                await _marketCapitalRepository.SaveChangesAsync();

                market.MarketCapitals.Add(marketCapitalDaily);
                _marketRepository.UpdateAsync(market);
                await _marketRepository.SaveChangesAsync();
                _cacheService.Set(market.GuidId, market);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalCreateError,
                    ErrorMessage = ErrorMessage.MarketCapitalCreateError
                };
            }
            return new BaseResult<MarketCapitalDto>
            {
                Data = _mapper.Map<MarketCapitalDto>(marketCapitalDaily),
            };
        }
        public async Task<BaseResult<MarketCapitalDto>> GetMarketCapitalAsync(string Day, string guidMarketId)
        {
            DateTime Date = DateTime.Parse(Day);
            var market = await _marketRepository.GetAll().Include(p=>p.MarketCapitals).ThenInclude(p=>p.ProductsSold).FirstOrDefaultAsync(p => p.GuidId == guidMarketId);
            if (market == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            
            var marketCapital = market.MarketCapitals.FirstOrDefault(p=>p.Date.DayOfYear.ToString() ==  Date.DayOfYear.ToString());
            if(marketCapital == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalNotFound,
                    ErrorMessage = ErrorMessage.MarketCapitalNotFound
                };
            }
            return new BaseResult<MarketCapitalDto>
            {
                Data = _mapper.Map<MarketCapitalDto>(marketCapital),
            };
        }
        public async Task<CollectionResult<ProductLineDto>> GetProductsLinesGuidAsync(string guidMarketId)
        {
            var market = await _marketRepository.GetAll()
                .Include(p=>p.MarketCapitals)
                .ThenInclude(p=>p.ProductsSold)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p=>p.GuidId == guidMarketId);
            if(market == null)
            {
                return new CollectionResult<ProductLineDto>
                {
                    ErrorCode= (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            if(market.RangeProducts == null)
            {
                market.RangeProducts = new List<ProductLine>();
                _marketRepository.UpdateAsync(market);
                await _marketRepository.SaveChangesAsync();
                _cacheService.Set(market.GuidId, market);
                return new CollectionResult<ProductLineDto>
                {
                    Data = new List<ProductLineDto>(),
                    Count = 0
                };
            }
            var productLinesDto = market.RangeProducts.Select(p => new ProductLineDto(p.Product.Name, p.Count.ToString()));
            return new CollectionResult<ProductLineDto>
            {
                Data= productLinesDto,
                Count = productLinesDto.Count()
            };
        }
        public async Task<BaseResult<MarketCapitalDto>> MinusProductLineInMarketAsync(string guidMarketId, string Day, string NameProduct)
        {
            var market = await _marketRepository.GetAll()
                .Include(p=>p.MarketCapitals)
                .ThenInclude(p=>p.ProductsSold)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == guidMarketId);
            if(market == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            var Data = DateTime.Parse(Day);
            var marketCapital = market.MarketCapitals.FirstOrDefault(p=>p.Date.DayOfYear.ToString() == Data.DayOfYear.ToString());
            if(marketCapital == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalNotFound,
                    ErrorMessage = ErrorMessage.MarketCapitalNotFound
                };
            }
            var prodl = marketCapital.ProductsSold.FirstOrDefault(p => p.Product.Name == NameProduct);
            if(prodl == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            if(prodl.Count < 0)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            try
            {
                prodl.Count--;
                _productLineRepository.UpdateAsync(prodl);
                await _productLineRepository.SaveChangesAsync();
                _cacheService.Set(prodl.GuidId, prodl);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalDeleteError,
                    ErrorMessage = ErrorMessage.MarketCapitalDeleteError,
                };
            }
            return new BaseResult<MarketCapitalDto>
            {
                Data = _mapper.Map<MarketCapitalDto>(marketCapital),
            };
        }
        public async Task<BaseResult<MarketCapitalDto>> PlusProductLineInMarketAsync(ProductLineNameDto dto)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.MarketCapitals)
                .ThenInclude(p => p.ProductsSold)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId ==dto.guidMarketId);
            if (market == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            var Data = DateTime.Parse(dto.Day);
            var marketCapital = market.MarketCapitals.FirstOrDefault(p => p.Date.Day.ToString() == Data.Day.ToString());
            if (marketCapital == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalNotFound,
                    ErrorMessage = ErrorMessage.MarketCapitalNotFound
                };
            }
            var pl = marketCapital.ProductsSold.FirstOrDefault(p => p.Product.Name == dto.NameProduct);
            if (pl == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            try
            {
                pl.Count++;
                _productLineRepository.UpdateAsync(pl);
                await _productLineRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineUpdateError,
                    ErrorMessage = ErrorMessage.ProductLineUpdateError,
                };
            }
            return new BaseResult<MarketCapitalDto>
            {
                Data = _mapper.Map<MarketCapitalDto>(marketCapital),
            };
        }
        public async Task<BaseResult<MarketCapitalDto>> RemoveProductLineInMarketAsync(string MarketGuidId, string Day, string NameProduct)
        {
            var market = await _marketRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == MarketGuidId);
            if (market == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            var Data = DateTime.Parse(Day);
            var marketCapital = market.MarketCapitals.FirstOrDefault(p => p.Date.DayOfYear.ToString() == Data.DayOfYear.ToString());
            if (marketCapital == null)
            {
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalNotFound,
                    ErrorMessage = ErrorMessage.MarketCapitalNotFound
                };
            }
            try
            {
                _marketCapitalRepository.DeleteAsync(marketCapital);
                await _marketCapitalRepository.SaveChangesAsync();
                _marketRepository.UpdateAsync(market);
                await _marketRepository.SaveChangesAsync();
                _cacheService.Set<Market>(market.GuidId,market);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MarketCapitalDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCapitalDeleteError,
                    ErrorMessage = ErrorMessage.MarketCapitalDeleteError,
                };
            }
            return new BaseResult<MarketCapitalDto>
            {
                Data = _mapper.Map<MarketCapitalDto>(marketCapital),
            };
        }
    }
}
