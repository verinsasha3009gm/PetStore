using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.Application.Resources;
using PetStore.Products.Domain.Dto.ProductTeg;
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

namespace PetStore.Products.Application.Services
{
    public class ProductTegService : IProductTegService
    {
        private readonly IBaseRepository<Teg> _TegRepository;
        private readonly IBaseRepository<Product> _ProductRepository;
        private readonly IBaseRepository<ProductTeg> _ProductTegRepository;
        private readonly IUnitOFWork _unitOFWork;
        public ProductTegService(IBaseRepository<Teg> TegRepository, IBaseRepository<Product> ProductRepository,
            IBaseRepository<ProductTeg> ProductTegRepository,IUnitOFWork unitOFWork)
        {
            _TegRepository = TegRepository;
            _ProductRepository = ProductRepository;
            _ProductTegRepository = ProductTegRepository;
            _unitOFWork = unitOFWork;
        }
        public async Task<BaseResult<ProductTegDto>> CreateProductTeg(ProductTegDto dto)
        {
            var prod = await _ProductRepository.GetAll().Include(p => p.Tegs)
                .FirstOrDefaultAsync(p => p.Name == dto.prodName);
            if (prod == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            var teg = await _TegRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.tegName);
            if (teg == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            if (prod.Tegs.FirstOrDefault(p => p.Name == teg.Name) == null)
            {
                var newProdTeg = new ProductTeg()
                {
                    ProductId = prod.Id,
                    TegId = teg.Id
                };
                await _ProductTegRepository.CreateAsync(newProdTeg);
                return new BaseResult<ProductTegDto>
                {
                    Data = new(prod.Name, teg.Name)
                };
            }
            return new BaseResult<ProductTegDto>()
            {
                ErrorCode = 44,
                ErrorMessage = "Error"
            };
        }

        public async Task<BaseResult<ProductTegDto>> DeleteProductTeg(string prodName, string teg)
        {
            var prod = await _ProductRepository.GetAll().Include(p => p.Tegs)
                .FirstOrDefaultAsync(p => p.Name == prodName);
            if (prod == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            var tegName = await _TegRepository.GetAll().FirstOrDefaultAsync(p => p.Name == teg);
            if (tegName == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            var prodTeg = await _ProductTegRepository.GetAll().Where(p => p.ProductId == prod.Id)
                .FirstOrDefaultAsync(p => p.TegId == tegName.Id);
            if (prodTeg == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            _ProductTegRepository.DeleteAsync(prodTeg);
            await _ProductTegRepository.SaveChangesAsync();
            return new BaseResult<ProductTegDto>()
            {
                Data = new ProductTegDto(prod.Name, tegName.Name)
            };
        }

        public async Task<BaseResult<ProductTegDto>> UpdateProductTeg(UpdateProductTegDto dto)
        {
            var prod = await _ProductRepository.GetAll().Include(p => p.Tegs)
                .FirstOrDefaultAsync(p => p.Name == dto.prodName);
            if (prod == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            var fromTegName = await _TegRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.fromTeg);
            if (fromTegName == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            var newTegName = await _TegRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.newTeg);
            if (newTegName == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            var prodTeg = await _ProductTegRepository.GetAll().Where(p => p.ProductId == prod.Id)
                .FirstOrDefaultAsync(p => p.TegId == fromTegName.Id);
            if (prodTeg == null)
            {
                return new BaseResult<ProductTegDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }

            using (var transaction = await _unitOFWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOFWork.ProductTegs.DeleteAsync(prodTeg);
                    await _ProductTegRepository.SaveChangesAsync();

                    prodTeg.TegId = newTegName.Id;
                    await _unitOFWork.ProductTegs.CreateAsync(prodTeg);

                    await _unitOFWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new BaseResult<ProductTegDto>()
                    {
                        Data = new ProductTegDto(dto.prodName, dto.newTeg)
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new BaseResult<ProductTegDto>()
                    {
                        ErrorMessage = ErrorMessages.ErrorAllTegs,
                        ErrorCode = (int)ErrorCodes.ErrorAllTegs,
                    };
                }
            }
        }
    }
}
