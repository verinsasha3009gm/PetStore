using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.Application.Resources;
using PetStore.Products.Domain.Dto.Description;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Enum;
using PetStore.Products.Domain.Interfaces.Repository;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PetStore.Products.Application.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IBaseRepository<Description> _descriptionRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IUnitOFWork _UnitOfWork;
        private readonly IMapper _mapper;
        public DescriptionService(IBaseRepository<Description> descriptionRepository,
            IBaseRepository<Product> productRepository, IMapper mapper,IUnitOFWork unitOFWork)
        {
            _descriptionRepository = descriptionRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _UnitOfWork = unitOFWork;
        }
        public async Task<BaseResult<DescriptionDto>> AddDescriptionCultureAsync(DescriptionCultureDto dto)
        {
            var product = await _productRepository.GetAll().Include(p=>p.DescriptionList).FirstOrDefaultAsync(p=>p.Name == dto.prodName);
            if (product == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode= (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound
                };
            }
            var culture = product.DescriptionList.FirstOrDefault(p=>p.Culture == dto.culture);
            if (culture != null) 
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode = (int)ErrorCodes.DescriptionNotFound,
                    ErrorMessage = ErrorMessages.DescriptionNotFound
                };
            }
            var description = new Description()
            {
                 Culture = dto.culture,
                 ProductId = product.Id,
                 Detail = dto.detail, 
                 ProductСomposition = dto.productСomposition,
            };
            using(var transaction= await _UnitOfWork.BeginTransitionAsync())
            {
                try
                {  
                    await _UnitOfWork.Descriptions.CreateAsync(description);
                    product.DescriptionList.Add(description);
                    _UnitOfWork.Products.UpdateAsync(product);
                    await _UnitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new BaseResult<DescriptionDto>
                    {
                        Data = _mapper.Map<DescriptionDto>(description)
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new BaseResult<DescriptionDto>()
                    {
                        ErrorCode = (int) ErrorCodes.ErrorCreateDescription,
                        ErrorMessage = ErrorMessages.ErrorCreateDescription,
                    };
                }
            }
        }

        public async Task<BaseResult<DescriptionDto>> GetDescriptionCultureAsync(string Name,string Culture)
        {
            var prod = await _productRepository.GetAll().Include(p=>p.DescriptionList).FirstOrDefaultAsync(p => p.Name == Name);
            if (prod == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound,
                };
            }
            var description = prod.DescriptionList.FirstOrDefault(p=>p.Culture == Culture);
            if(description == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode = (int) ErrorCodes.DescriptionNotFound,
                    ErrorMessage = ErrorMessages.DescriptionNotFound,
                };
            }
            return new BaseResult<DescriptionDto>
            {
                Data = _mapper.Map<DescriptionDto>(description),
                
            };
        }

        public async Task<CollectionResult<DescriptionDto>> GetDescriptionsAsync(string prodName)
        {
            var prod = await _productRepository.GetAll()
                .Include(p => p.DescriptionList)
                .FirstOrDefaultAsync(p => p.Name == prodName);
            if (prod == null)
            {
                return new CollectionResult<DescriptionDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorGetDescription,
                    ErrorMessage = ErrorMessages.ErrorGetDescription
                };
            }
            var descriptions = prod.DescriptionList.Select(p=>new DescriptionDto(p.Culture, p.Detail)).ToList();
            return new CollectionResult<DescriptionDto>
            {
                Data = descriptions
            };
        }

        public async Task<BaseResult<DescriptionDto>> RemoveDescriptionCultureAsync(string prodName, string culture)
        {
            var prod = await _productRepository.GetAll().Include(p=>p.DescriptionList).FirstOrDefaultAsync(p=>p.Name== prodName);
            if(prod == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode = (int) ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound
                };
            }
            var description = prod.DescriptionList.FirstOrDefault(p=>p.Culture == culture);
            if(description == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode = (int)ErrorCodes.DescriptionNotFound,
                    ErrorMessage = ErrorMessages.DescriptionNotFound
                };
            }

            using (var transaction = await _UnitOfWork.BeginTransitionAsync())
            {
                try
                {
                    prod.DescriptionList.Remove(description);
                    _UnitOfWork.Products.UpdateAsync(prod);
                    _UnitOfWork.Descriptions.DeleteAsync(description);
                    await _UnitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();   
                    return new BaseResult<DescriptionDto>
                    {
                        Data = _mapper.Map<DescriptionDto>(description)
                    };                                              
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new BaseResult<DescriptionDto>()
                    {
                        ErrorCode = (int)ErrorCodes.ErrorDeleteDescription,
                        ErrorMessage = ErrorMessages.ErrorDeleteDescription
                    };
                }
            }
        }

        public async Task<BaseResult<DescriptionDto>> UpdateDescriptionCultureAsync(DescriptionCultureDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.prodName);
            if(prod == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound,
                };
            }
            var description = prod.DescriptionList.FirstOrDefault(p=>p.Culture == dto.culture);
            if(description == null)
            {
                return new BaseResult<DescriptionDto>
                {
                    ErrorMessage = ErrorMessages.DescriptionNotFound,
                    ErrorCode = (int)ErrorCodes.DescriptionNotFound,
                };
            }
            try
            {   
                description.Culture = dto.culture;
                description.Detail = dto.detail;
                description.ProductСomposition= dto.productСomposition;
                _descriptionRepository.UpdateAsync(description);
                await _descriptionRepository.SaveChangesAsync();
            }
            catch
            {
                return new BaseResult<DescriptionDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorUpdateDescription,
                    ErrorMessage = ErrorMessages.ErrorUpdateDescription
                };
            }
            return new BaseResult<DescriptionDto>
            {
                Data = _mapper.Map<DescriptionDto>(description)
            };
        }
    }
}
