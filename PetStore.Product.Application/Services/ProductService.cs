using PetStore.Products.Domain.Dto;
using PetStore.Products.Domain.Interfaces.Repository;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using PetStore.Products.Application.Resources;
using PetStore.Products.Domain.Enum;
using AutoMapper;
using Petstore.Products.Producer.Interfaces;
using PetStore.Products.Domain.Settings;
using Microsoft.Extensions.Options;

namespace PetStore.Products.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IMessageProducer _messageProducer;
        private readonly IOptions<RabbitMqSettings> _rabbitOptions;
        public ProductService(IBaseRepository<Product> productRepository,
            IBaseRepository<Category> categoryRepository, IMapper mapper,
            ICacheService cacheService, IMessageProducer messageProducer,
            IOptions<RabbitMqSettings> rabbitOptions)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _messageProducer = messageProducer;
            _rabbitOptions = rabbitOptions;
        }
        public async Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if (prod != null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage = ErrorMessages.ProductAlreadyExists,
                };
            }
            try
            {
                prod = new() { Name = dto.Name , Description = dto.Description , Price = dto.Price, GuidId = Guid.NewGuid().ToString()
                    , CategoryId = dto.CategoryId ,ProductPassportId= 1};
                await _productRepository.CreateAsync(prod);
                _cacheService.Set(prod.GuidId, prod);
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorCreateProduct,
                    ErrorMessage = ErrorMessages.ErrorCreateProduct,
                };
            }
            _messageProducer.SendMessage(prod, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
            _messageProducer.SendMessage(prod, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod)
            };
        }

        public async Task<BaseResult<ProductDto>> DeleteProductAsync(string name)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Name == name);
            if (prod == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound,
                };
            }
            try
            {
                _productRepository.DeleteAsync(prod);
                await _productRepository.SaveChangesAsync();
                _cacheService.Delete<Product>(prod.GuidId);

            }
            catch
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorDeleteProduct,
                    ErrorMessage = ErrorMessages.ErrorDeleteProduct,
                };
            }
            _messageProducer.SendMessage(prod, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
            _messageProducer.SendMessage(prod, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
            return new BaseResult<ProductDto>()
            {
                Data = _mapper.Map<ProductDto>(prod),
            };
        }

        public async Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.GuidId);
            if (prod == null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound,
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

                _messageProducer.SendMessage(prod, nameof(HttpMethods.PUT), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                _messageProducer.SendMessage(prod, nameof(HttpMethods.PUT), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                return new BaseResult<ProductDto>()
                {
                    Data = new ProductDto (prod.Name, prod.Description,prod.Price)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorUpdateProduct,
                    ErrorMessage = ErrorMessages.ErrorUpdateProduct,
                };
            }
        }
        public async Task<BaseResult<ProductGuidDto>> GetProductAsync(string name)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == name);
            if (prod == null)
            {
                return new BaseResult<ProductGuidDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorGetProduct,
                    ErrorMessage = ErrorMessages.ErrorGetProduct
                };
            }
            _cacheService.Refrech<Product>(prod.GuidId);
            return new BaseResult<ProductGuidDto>()
            {
                Data = _mapper.Map<ProductGuidDto>(prod)
            };
        }

        public async Task<CollectionResult<ProductDto>> GetAllProductInCategory(string categoryName)
        {
            var prodDtos = await _categoryRepository.GetAll().Include(p=>p.Products)
                .FirstOrDefaultAsync(p=>p.Name == categoryName);
            if(prodDtos == null)
            {
                return new CollectionResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryIsNotFound,
                    ErrorMessage = ErrorMessages.CategoryIsNotFound,
                };
            }
            var dtos = prodDtos.Products.Select(p=>new ProductDto(p.Name,p.Description, p.Price)).ToList();
            if(dtos == null)
            {
                return new CollectionResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorGetAllProductsInCategory,
                    ErrorMessage = ErrorMessages.ErrorGetAllProductsInCategory,
                };
            }
            return new CollectionResult<ProductDto>()
            {
                Data = dtos
            };
        }

        public async Task<CollectionResult<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAll().Select(p=> new ProductDto(p.Name,p.Description, p.Price)).ToArrayAsync();
            if (products == null)
            {
                return new CollectionResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorGetAllProduct,
                    ErrorMessage = ErrorMessages.ErrorGetAllProduct,
                };
            }
            return new CollectionResult<ProductDto>()
            {
                Data = products
            };
        }

        public Task<CollectionResult<ProductDto>> GetAllTeg(string tegName)
        {
            throw new NotImplementedException();
        }
    }
}
