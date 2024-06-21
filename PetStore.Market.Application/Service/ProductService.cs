using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Domain.Dto.Passport;
using PetStore.Markets.Domain.Dto.Product;
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
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _prodRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public ProductService(IBaseRepository<Product> prodRepository, IMapper mapper, ILogger logger, ICacheService cacheService)
        {
            _prodRepository = prodRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<BaseResult<Product>> CreateProductInRabbit(Product product)
        {
            product.Id = 0;
            var prod = await _prodRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == product.GuidId);
            if (prod != null)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage = ErrorMessage.ProductAlreadyExists,
                };
            }
            try
            {
                await _prodRepository.CreateAsync(product);
                _cacheService.Set(product.GuidId, product);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<Product>()
            {
                Data = product
            };
        }

        public async Task<BaseResult<ProductDto>> UpdateProductInRabbit(Product dto)
        {
            var prod = await _prodRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.GuidId);
            if (prod == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound,
                };
            }
            try
            {
                prod.Name = dto.Name;
                prod.Description = dto.Description;
                prod.Price = dto.Price;
                _prodRepository.UpdateAsync(prod);
                await _prodRepository.SaveChangesAsync();
                _cacheService.Set(prod.GuidId, prod);
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }
        public async Task<BaseResult<ProductDto>> DeleteProductInRabbit(string Guid)
        {
            var prod = await _prodRepository.GetAll()
                .FirstOrDefaultAsync(p => p.GuidId == Guid);
            if (prod == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound,
                };
            }
            try
            {
                _prodRepository.DeleteAsync(prod);
                await _prodRepository.SaveChangesAsync();
                //_cacheService.Delete<Product>(prod.GuidId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }

        public async Task<BaseResult<ProductGuidDto>> CreateProductAsync(ProductDto ProductDto)
        {
            var prod = await _prodRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == ProductDto.Name);
            if (prod != null)
            {
                return new BaseResult<ProductGuidDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage = ErrorMessage.ProductAlreadyExists,
                };
            }
            try
            {
                prod = new Product()
                {
                    Name = ProductDto.Name,
                    Description = ProductDto.Description,
                    GuidId = Guid.NewGuid().ToString(),
                    Price = ProductDto.Price,
                    ProductLines = new List<ProductLine>()
                };
                await _prodRepository.CreateAsync(prod);
                _cacheService.Set(prod.GuidId, prod);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductCreateError,
                    ErrorMessage = ErrorMessage.ProductCreateError
                };
            }
            return new BaseResult<ProductGuidDto>()
            {
                Data = _mapper.Map<ProductGuidDto>(prod),
            };
        }

        public async Task<BaseResult<ProductDto>> DeleteProductAsync(string ProductGuidId)
        {
            var prod = await _prodRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == ProductGuidId);
            if (prod == null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            
            try
            {
                _prodRepository.DeleteAsync(prod);
                await _prodRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductDeleteError,
                    ErrorMessage =ErrorMessage.ProductDeleteError
                };
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }

        public async Task<BaseResult<ProductDto>> GetProductAsync(string ProductGuidId)
        {
            var prod = _cacheService.Get<Product>(ProductGuidId);
            if (prod == null)
            {
                prod = await _prodRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == ProductGuidId);
                if (prod == null)
                {
                    return new BaseResult<ProductDto>
                    {
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                        ErrorMessage = ErrorMessage.ProductNotFound
                    };
                }
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }

        public async Task<BaseResult<ProductDto>> UpdateProductAsync(ProductGuidDto ProductDto)
        {
            var prod = await _prodRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == ProductDto.GuidId);
            if (prod == null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            try
            {
                prod.Name = ProductDto.Name;
                prod.Description = ProductDto.Description;
                prod.Price = ProductDto.Price;
                _prodRepository.UpdateAsync(prod);
                await _prodRepository.SaveChangesAsync();
                _cacheService.Set(prod.GuidId, prod);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductUpdateError,
                    ErrorMessage = ErrorMessage.ProductUpdateError
                };
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }
    }
}
