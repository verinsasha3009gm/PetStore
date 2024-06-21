using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
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
    public class ProductLineService : IProductLineService
    {
        private readonly IBaseRepository<ProductLine> _productLineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public ProductLineService(IBaseRepository<ProductLine> productLineRepository, IMapper mapper, ILogger logger, ICacheService cacheService)
        {
            _productLineRepository = productLineRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<ProductLineDto>> GetProductLineAsync(string GuidId)
        {
            var prodLine = _cacheService.Get<ProductLine>(GuidId);
            if(prodLine == null)
            {
                prodLine = await _productLineRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == GuidId);
                if(prodLine == null)
                {
                    return new BaseResult<ProductLineDto>
                    {
                        ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                        ErrorMessage = ErrorMessage.ProductLineNotFound,
                    };
                }

            }
            _cacheService.Refrech<ProductLine>(prodLine.GuidId);
            return new BaseResult<ProductLineDto>
            {
                Data = new ProductLineDto(prodLine.Product.Name, prodLine.Count.ToString())
            };
        }
        public async Task<BaseResult<ProductLineDto>> MinusProductLineAsync(string GuidId)
        {
            var prodLine = await _productLineRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == GuidId);
            if (prodLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound,
                };
            }
            
            if(prodLine.Count <1)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            try
            {
                prodLine.Count--;
                _productLineRepository.UpdateAsync(prodLine);
                await _productLineRepository.SaveChangesAsync();
                _cacheService.Set(prodLine.GuidId, prodLine);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<ProductLineDto>
                {
                    ErrorMessage = ErrorMessage.PassportUpdateError,
                    ErrorCode = (int)ErrorCodes.PassportUpdateError
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = new ProductLineDto(prodLine.Product.Name,prodLine.Count.ToString())
            };
        }
        public async Task<BaseResult<ProductLineDto>> PlusProductLineAsync(ProductLineGuidDto dto)
        {
            var prodLine = await _productLineRepository.GetAll().Include(p=>p.Product).FirstOrDefaultAsync(p => p.GuidId == dto.GuidId);
            if (prodLine == null)
            {
                return new BaseResult<ProductLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductLineNotFound,
                    ErrorMessage = ErrorMessage.ProductLineNotFound,
                };
            }
            try
            {
                prodLine.Count++;
                _productLineRepository.UpdateAsync(prodLine);
                await _productLineRepository.SaveChangesAsync();
                _cacheService.Set(prodLine.GuidId, prodLine);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<ProductLineDto>
                {
                    ErrorMessage = ErrorMessage.PassportUpdateError,
                    ErrorCode = (int)ErrorCodes.PassportUpdateError
                };
            }
            return new BaseResult<ProductLineDto>
            {
                Data = new ProductLineDto(prodLine.Product.Name, prodLine.Count.ToString())
            };
        }
    }
}
