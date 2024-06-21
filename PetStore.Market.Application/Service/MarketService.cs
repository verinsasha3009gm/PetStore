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

namespace PetStore.Markets.Application.Service
{
    public class MarketService : IMarketService
    {
        private readonly IBaseRepository<Market> _marketRepository;
        private readonly IBaseRepository<ProductLine> _productLineRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<MarketCapital> _marketCapitalRepository;
        private readonly IMarketCapitalService _marketCapitalService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public MarketService(IBaseRepository<Market> marketRepository, IBaseRepository<ProductLine> productLineRepository
            , IBaseRepository<Product> productRepository, IBaseRepository<MarketCapital> marketCapitalRepository
            , IMarketCapitalService marketCapitalService, IMapper mapper, ILogger logger, ICacheService cacheService)
        {
            _marketRepository = marketRepository;
            _productLineRepository = productLineRepository;
            _productRepository = productRepository;
            _marketCapitalRepository = marketCapitalRepository;
            _marketCapitalService = marketCapitalService;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<ProductLineDto>> AddProductInMarket(MarketProductLineDto dto)
        {
            var market = await _marketRepository.GetAll()
                .Include(p=>p.RangeProducts)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == dto.MarketGuidId);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.NameProduct);
            if (prod == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound,
                };
            }
            var ProductLine = market.RangeProducts.FirstOrDefault(p=>p.Product.Name == dto.NameProduct);
            if (ProductLine == null)
            {
                ProductLine = new ProductLine
                {
                    Count = (int)dto.Count,
                    Product = prod,
                    GuidId = Guid.NewGuid().ToString(),
                };
                try
                {
                    await _productLineRepository.CreateAsync(ProductLine);
                    _cacheService.Set(ProductLine.GuidId,ProductLine);
                    market.RangeProducts.Add(ProductLine);
                    _marketRepository.UpdateAsync(market);
                    await _marketRepository.SaveChangesAsync();
                    _cacheService.Set(market.GuidId, market);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<ProductLineDto>
                    {
                        ErrorMessage = ErrorMessage.ProductLineCreateError,
                        ErrorCode = (int)ErrorCodes.ProductLineCreateError
                    };
                }
                return new BaseResult<ProductLineDto>
                {
                    Data = new ProductLineDto(dto.NameProduct, dto.Count.ToString())
                };
            }
            try
            {
                ProductLine.Count += int.Parse(dto.Count.ToString());
                _productLineRepository.UpdateAsync(ProductLine);
                await _productLineRepository.SaveChangesAsync();
                _cacheService.Set(ProductLine.GuidId, ProductLine);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineUpdateError,
                    ErrorMessage = ErrorMessage.ProductLineUpdateError
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = _mapper.Map<ProductLineDto>(ProductLine)
            };
        }
        public async Task<BaseResult<MarketDto>> GetMarketGuidAsync(string guidId)
        {
            var market = await _marketRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
            if (market == null)
            {
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            return new BaseResult<MarketDto>
            {
                Data = _mapper.Map<MarketDto>(market)
            };
        }
        //
        public async Task<BaseResult<ProductLineDto>> GetProductGuidInMarketAsync(string productGuid, string MarketName)
        {
            var market = await _marketRepository.GetAll()
                .Include(p=>p.RangeProducts)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p => p.Name == MarketName);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p=>p.Product.GuidId == productGuid);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = _mapper.Map<ProductLineDto>(productLine),
            };
        }

        public async Task<BaseResult<ProductLineDto>> GetProductGuidInMarketGuidAsync(string productGuid, string MarketGuid)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.RangeProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == MarketGuid);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p => p.Product.GuidId== productGuid);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = _mapper.Map<ProductLineDto>(productLine)
            };
        }

        public async Task<BaseResult<ProductLineDto>> GetProductInMarketAsync(string productName, string MarketName)
        {
            var market = await _marketRepository.GetAll()
                .Include(p=>p.RangeProducts)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p=>p.Name == MarketName);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p => p.Product.Name == productName);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            _cacheService.Refrech<ProductLine>(productLine.GuidId);
            return new BaseResult<ProductLineDto>
            {
                Data = _mapper.Map<ProductLineDto>(productLine),
            };
        }

        public async Task<BaseResult<ProductLineDto>> GetProductInMarketGuidAsync(string productName, string MarketGuid)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.RangeProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == MarketGuid);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p => p.Product.Name == productName);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            //_cacheService.Refrech<ProductLine>(productLine.GuidId);
            return new BaseResult<ProductLineDto>
            {
                Data = _mapper.Map<ProductLineDto>(productLine),
            };
        }

        public async Task<BaseResult<ProductLineDto>> MinusProductInMarketAsync(string MarketGuid, string NameProduct)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.RangeProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == MarketGuid);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p => p.Product.Name == NameProduct);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            if(productLine.Count <= 0)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            try
            {
                productLine.Count--;
                _productLineRepository.UpdateAsync(productLine);
                await _productLineRepository.SaveChangesAsync();
                _cacheService.Set(productLine.GuidId, productLine);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineUpdateError,
                    ErrorMessage = ErrorMessage.ProductLineUpdateError
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = _mapper.Map<ProductLineDto>(productLine)
            };
        }

        public async Task<BaseResult<ProductLineDto>> PlusProductInMarketAsync(MarketProductLineDto dto)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.RangeProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == dto.MarketGuidId);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p => p.Product.Name == dto.NameProduct);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            try
            {
                productLine.Count++;
                _productLineRepository.UpdateAsync(productLine);
                await _productLineRepository.SaveChangesAsync();
                _cacheService.Set(productLine.GuidId, productLine);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineUpdateError,
                    ErrorMessage = ErrorMessage.ProductLineUpdateError
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = new ProductLineDto(productLine.Product.Name,productLine.Count.ToString())
            };
        }

        public async Task<BaseResult<ProductLineDto>> RemoveProductInMarketAsync(string MarketGuid, string NameProduct)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.RangeProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == MarketGuid);
            if (market == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var productLine = market.RangeProducts.FirstOrDefault(p => p.Product.Name == NameProduct);
            if (productLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound
                };
            }
            try
            {
                _productLineRepository.DeleteAsync(productLine);
                await _productLineRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineDeleteError,
                    ErrorMessage = ErrorMessage.ProductLineDeleteError
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data =_mapper.Map<ProductLineDto>(productLine),
            };
        }

        public async Task<BaseResult<MarketDto>> CreateMarketAsync(CreateMarketDto dto)
        {
            var market = await _marketRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if (market != null)
            {
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            try
            {
                market = new()
                {
                    Name = dto.Name,
                    GuidId = Guid.NewGuid().ToString(),
                    Employes = new List<Employe>(),
                    MarketCapitals = new(),
                    RangeProducts =new(),
                };
                await _marketRepository.CreateAsync(market);
                _cacheService.Set(market.GuidId, market);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketCreateError,
                    ErrorMessage = ErrorMessage.MarketCreateError
                };
            }
            return new BaseResult<MarketDto>
            {
                Data = _mapper.Map<MarketDto>(market),
            };
        }

        public async Task<BaseResult<MarketDto>> DeleteMarketAsync(string guidId)
        {
            var market = await _marketRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
            if (market == null)
            {
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            try
            {
                _marketRepository.DeleteAsync(market);
                await _marketRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketDeleteError,
                    ErrorMessage = ErrorMessage.MarketDeleteError
                };
            }
            return new BaseResult<MarketDto>
            {
                Data =_mapper.Map<MarketDto>(market),
            };
        }

        public async Task<BaseResult<MarketDto>> GetMarketAsync(string MarketName)
        {
            var market = await _marketRepository.GetAll().FirstOrDefaultAsync(p => p.Name == MarketName);
            if (market == null)
            {
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            _cacheService.Refrech<Market>(market.GuidId);
            return new BaseResult<MarketDto>
            {
                Data = _mapper.Map<MarketDto>(market),
            };
        }
        public async Task<BaseResult<MarketDto>> UpdateMarketAsync(MarketUpdateDto dto)
        {
            var market = await _marketRepository.GetAll()
                .Include(p => p.RangeProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.GuidId == dto.guidId);
            if (market == null)
            {
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            try
            {
                market.Name = dto.MarketName;
                _marketRepository.UpdateAsync(market);
                await _marketRepository.SaveChangesAsync();
                _cacheService.Set(market.GuidId, market);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MarketDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketUpdateError,
                    ErrorMessage = ErrorMessage.MarketUpdateError
                };
            }
            return new BaseResult<MarketDto>
            {
                Data = _mapper.Map<MarketDto>(market),
            };
        }
    }
}
