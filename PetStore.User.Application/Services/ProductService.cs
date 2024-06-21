using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Users.Application.Resources;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Enum;
using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Settings;
using Serilog;
using System;
using System.Xml.Linq;

namespace PetStore.Users.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public ProductService(IBaseRepository<Product> productRepository,
            IMapper mapper,ILogger logger, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<Product>> CreateProductInRabbit(Product product)
        {
            var prod = new Product()
            {
                GuidId = product.GuidId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductPassportId = product.ProductPassportId,
                CategoryId = product.CategoryId
            };
            var prod2 = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == prod.GuidId);
            if (prod2 != null)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage = ErrorMessage.ProductAlreadyExists,
                };
            }
            try
            {
                await _productRepository.CreateAsync(prod);
                _cacheService.Set(prod.GuidId, prod);
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
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.GuidId);
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
                _productRepository.UpdateAsync(prod);
                await _productRepository.SaveChangesAsync();
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
            var prod = await _productRepository.GetAll()
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
                _productRepository.DeleteAsync(prod);
                await _productRepository.SaveChangesAsync();
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

        public async Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if (prod != null )
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage=ErrorMessage.ProductAlreadyExists,
                };
            }            var guidId = Guid.NewGuid().ToString();
            prod = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                GuidId = guidId
            };
            try
            {
                await _productRepository.CreateAsync(prod);
                _cacheService.Set(prod.GuidId, prod);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod),
            };
        }
        public async Task<BaseResult<ProductDto>> DeleteProductAsync(string name)
        {
            var prod = await _productRepository.GetAll()
                .FirstOrDefaultAsync(p => p.Name == name);
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
                _productRepository.DeleteAsync(prod);
                await _productRepository.SaveChangesAsync();
                //_cacheService.Delete<Product>(prod.GuidId);
            }
            catch(Exception ex)
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
        public async Task<CollectionResult<ProductDto>> GetAllProductsAsync()
        {
            var prods = await _productRepository.GetAll().Select(p=>new ProductDto(p.Name,p.Description,p.Price,p.GuidId)).ToListAsync();
            if(prods == null)
            {
                return new CollectionResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            return new CollectionResult<ProductDto>()
            {
                Data = prods,
                Count = prods.Count()
            };
        }
        public async Task<BaseResult<ProductDto>> GetProductAsync(string name)
        {
            var prod = await _productRepository.GetAll()
                .FirstOrDefaultAsync(p=>p.Name == name);
            if(prod == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            _cacheService.Get<Product>(prod.GuidId);
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod )
            };
        }
        public async Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.LastName);
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
                prod.Name = dto.NewName;
                prod.Description = dto.Description;
                prod.Price = dto.Price;
                _productRepository.UpdateAsync(prod);
                await _productRepository.SaveChangesAsync();
                _cacheService.Set<Product>(prod.GuidId, prod);
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
        public async Task<BaseResult<ProductDto>> GetProductGuidIdAsync(string guidId)
        {
            var prod = _cacheService.Get<Product>(guidId);
            if (prod == null)
            {
                prod = await _productRepository.GetAll()
                .FirstOrDefaultAsync(p => p.GuidId == guidId);
                if (prod == null)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                        ErrorMessage = ErrorMessage.ProductNotFound
                    };
                }
                _cacheService.Get<Product>(guidId);
            }
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }
        public async Task<BaseResult<ProductDto>> DeleteProductGuidIdAsync(string guidId)
        {
            var prod = await _productRepository.GetAll()
            .FirstOrDefaultAsync(p => p.GuidId == guidId);
            if (prod == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            _productRepository.DeleteAsync(prod);
            await _productRepository.SaveChangesAsync();
            
            //_cacheService.Delete<Product>(guidId);
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }
        public async Task<BaseResult<ProductGuidDto>> GetProductGuidAsync(string name)
        {
            var prod = await _productRepository.GetAll()
            .FirstOrDefaultAsync(p => p.Name == name);
            if (prod == null)
            {
                return new BaseResult<ProductGuidDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessage.ProductNotFound
                };
            }
            _cacheService.Refrech<Product>(prod.GuidId);
            return new BaseResult<ProductGuidDto>()
            {
                Data = new(prod.GuidId, prod.Name,prod.Description,prod.Price)
            };
        }
    }
}
