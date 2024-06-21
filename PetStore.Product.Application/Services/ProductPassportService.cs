using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.Application.Resources;
using PetStore.Products.Domain.Dto.ProductPassport;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Enum;
using PetStore.Products.Domain.Interfaces.Repository;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.Application.Services
{
    public class ProductPassportService : IProductPassportService
    {
        private readonly IBaseRepository<ProductPassport> _productPassportRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public ProductPassportService(IBaseRepository<ProductPassport> productPassport, IBaseRepository<Product> productRepository,
            IMapper mapper, ICacheService cacheService)
        {
            _productPassportRepository = productPassport;
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<ProductPassportDto>> CreateProductPassportAsync(ProductPassportDto dto)
        {
            var prodPassport = await _productPassportRepository.GetAll().FirstOrDefaultAsync(p=>p.Company == dto.company);
            if (prodPassport != null)
            {
                return new BaseResult<ProductPassportDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportAlreadyExists,
                    ErrorMessage =ErrorMessages.ProductPassportAlreadyExists
                };
            }
            prodPassport = new ProductPassport()
            {
                Company = dto.company,
                Name = dto.name,
                Description = dto.description,
                GuidId = Guid.NewGuid().ToString(),
            };

            await _productPassportRepository.CreateAsync(prodPassport);
            _cacheService.Set<ProductPassport>(prodPassport.GuidId, prodPassport);
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(prodPassport)
            };
        }

        public async Task<BaseResult<ProductPassportDto>> DeleteProductPassportAsync(string Name, string Company)
        {
            var prodPassport = await _productPassportRepository.GetAll()
                .Where(p=>p.Company == Company)
                .FirstOrDefaultAsync(p => p.Name == Name);
            if (prodPassport == null)
            {
                return new BaseResult<ProductPassportDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound,
                    ErrorMessage = ErrorMessages.ProductPassportNotFound
                };
            }
            _productPassportRepository.DeleteAsync(prodPassport);
            await _productPassportRepository.SaveChangesAsync();
            _cacheService.Delete<ProductPassport>(prodPassport.GuidId);
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(prodPassport) 
            };
        }

        public async Task<BaseResult<ProductPassportDto>> GetProductPassportAsync(string Company,string prodPassportName)
        {
            var prodPassport = await _productPassportRepository.GetAll()
                .Where(p => p.Company == Company)
                .FirstOrDefaultAsync(p => p.Name == prodPassportName);
            if (prodPassport == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound,
                    ErrorMessage =ErrorMessages.ProductPassportNotFound
                };
            }
            _cacheService.Refrech<ProductPassport>(prodPassport.GuidId);
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(prodPassport)
            };
        }

        public async Task<BaseResult<ProductPassportDto>> UpdateProductPassportAsync(UpdateProductPassportDto dto)
        {
            var prodPassport = await _productPassportRepository
                .GetAll()
                .Where(p=>p.Company == dto.LastCompany)
                .FirstOrDefaultAsync(p=>p.Name == dto.LastName);
            if(prodPassport == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound ,
                    ErrorMessage = ErrorMessages.ProductPassportNotFound
                };
            }
            try
            {
                prodPassport.Company = dto.newCompany;
                prodPassport.Description = dto.newDescription;
                prodPassport.Name = dto.newName;
                _productPassportRepository.UpdateAsync(prodPassport);
                await _productPassportRepository.SaveChangesAsync();

                _cacheService.Set(prodPassport.GuidId, prodPassport);
                
            }
            catch(Exception ex)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorUpdateProductPassport,
                    ErrorMessage = ErrorMessages.ErrorUpdateProductPassport
                };
            }
            return new BaseResult<ProductPassportDto>()
            {
                Data = _mapper.Map<ProductPassportDto>(prodPassport) 
            };
        }

        public async Task<BaseResult<ProductPassportDto>> GetPassportInProductAsync(string nameProduct, string nameProductPassport, string companyProductPassport)
        {
            var prod = await _productRepository.GetAll().Include(p=>p.ProductPassport).FirstOrDefaultAsync(p=>p.Name == nameProduct);
            if(prod == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode=(int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound
                };
            }
            var productPassport = await _productPassportRepository
               .GetAll()
               .Where(p=>p.Company == companyProductPassport)
               .FirstOrDefaultAsync(p=>p.Name == nameProductPassport);
            if(productPassport == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound,
                    ErrorMessage = ErrorMessages.ProductPassportNotFound
                };
            }
            if(prod.ProductPassport.Id != productPassport.Id)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorGetProductPassport,
                    ErrorMessage = ErrorMessages.ErrorGetProductPassport
                };
            }
            _cacheService.Refrech<ProductPassport>(productPassport.GuidId);
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(productPassport)
            };
        }
        public async Task<BaseResult<ProductPassportDto>> AddPassportInProductAsync(ProductInProductPassportDto dto)
        {
            var prod = await _productRepository.GetAll().Include(p => p.ProductPassport).FirstOrDefaultAsync(p => p.Name == dto.nameProduct);
            if (prod == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage = ErrorMessages.ProductAlreadyExists
                };
            }
            var productPassport = await _productPassportRepository
               .GetAll()
               .Where(p => p.Company == dto.companyProductPassport)
               .FirstOrDefaultAsync(p => p.Name == dto.nameProduct);
            if (productPassport == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound,
                    ErrorMessage = ErrorMessages.ProductPassportNotFound
                };
            }
            prod.ProductPassport = productPassport;
            _productRepository.UpdateAsync(prod);
            await _productRepository.SaveChangesAsync();

            productPassport.ProductId = prod.Id;
            _productPassportRepository.UpdateAsync(productPassport);
            await _productPassportRepository.SaveChangesAsync();

            _cacheService.Set(prod.GuidId, prod);
            _cacheService.Set(productPassport.GuidId, productPassport);
            
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(productPassport) 
            };
        }
        public async Task<BaseResult<ProductPassportDto>> UpdatePassportInProductAsync(ProductInProductPassportDto dto)
        {
            var prod = await _productRepository.GetAll().Include(p => p.ProductPassport).FirstOrDefaultAsync(p => p.Name == dto.nameProduct);
            if (prod == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound,
                };
            }
            var productPassport = await _productPassportRepository
               .GetAll()
               .Where(p => p.Company == dto.companyProductPassport)
               .FirstOrDefaultAsync(p => p.Name == dto.nameProductPassport);
            if (productPassport == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound,
                    ErrorMessage = ErrorMessages.ProductPassportNotFound
                };
            }
            prod.ProductPassport = productPassport;
            _productRepository.UpdateAsync(prod);
            await _productRepository.SaveChangesAsync();
            _cacheService.Set(prod.GuidId, prod);
            _cacheService.Set(productPassport.GuidId, productPassport);
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(productPassport)
            };
        }
        public async Task<BaseResult<ProductPassportDto>> RemovePassportInProductAsync(string nameProduct, string nameProductPassport, string companyProductPassport)
        {
            var prod = await _productRepository.GetAll().Include(p => p.ProductPassport).FirstOrDefaultAsync(p => p.Name == nameProduct);
            if (prod == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound
                };
            }
            var productPassport = await _productPassportRepository
               .GetAll()
               .Where(p => p.Company == companyProductPassport)
               .FirstOrDefaultAsync(p => p.Name == nameProductPassport);

            if (productPassport == null)
            {
                return new BaseResult<ProductPassportDto>()
                {
                    ErrorCode = (int)ErrorCodes.ProductPassportNotFound,
                    ErrorMessage = ErrorMessages.ProductPassportNotFound
                };
            }
            prod.ProductPassport = null;
            _productRepository.UpdateAsync(prod);
            await _productRepository.SaveChangesAsync(); 
            var Data = _cacheService.Get<Product>(prod.GuidId);
            _cacheService.Set(prod.GuidId, prod);
            
            return new BaseResult<ProductPassportDto>
            {
                Data = _mapper.Map<ProductPassportDto>(productPassport)
            };
        }
    }
}
