using PetStore.Products.Domain.Dto.Category;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;
using PetStore.Products.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.Domain.Enum;
using PetStore.Products.Application.Resources;
using AutoMapper;

namespace PetStore.Products.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> _CategoryRepository;
        private readonly IBaseRepository<Product> _ProductRepository;
        private readonly IUnitOFWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IBaseRepository<Category> categoryRepository,
            IBaseRepository<Product> productRepository, IMapper mapper,IUnitOFWork unitOFWork)
        {
            _CategoryRepository = categoryRepository;
            _ProductRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOFWork;
        }

        public async Task<BaseResult<CategoryDto>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = await _CategoryRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if (category != null)
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryAlreadyExists,
                    ErrorMessage = ErrorMessages.CategoryAlreadyExists
                };
            }
            category = new Category()
            {
                Description= dto.Description,
                Name = dto.Name,
                Products = new List<Product>()
            };
            try
            {
                await _CategoryRepository.CreateAsync(category);
            }
            catch (Exception ex)
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorCreateCategory,
                    ErrorMessage = ErrorMessages.ErrorCreateCategory
                };
            }
            return new BaseResult<CategoryDto>()
            {
                Data = _mapper.Map<CategoryDto>(category),
            };
        }

        public async Task<BaseResult<CategoryDto>> DeleteCategoryAsync(string nameCategory)
        {
            var category = await _CategoryRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == nameCategory);
            if (category == null)
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorDeleteCategory,
                    ErrorMessage = ErrorMessages.ErrorDeleteCategory
                };
            }
            _CategoryRepository.DeleteAsync(category);
            await _CategoryRepository.SaveChangesAsync();
            return new BaseResult<CategoryDto>()
            {
                Data = _mapper.Map<CategoryDto>(category),
            };
        }

        public async Task<CollectionResult<CategoryDto>> GetAllCategories()
        {
            var categories = await _CategoryRepository.GetAll().Select(p => new CategoryDto(p.Name)).ToArrayAsync();
            if(categories == null)
            {
                return new CollectionResult<CategoryDto>()
                {
                    ErrorCode = 44,
                    ErrorMessage = "Error"
                };
            }
            return new CollectionResult<CategoryDto>()
            {
                Data = categories
            };
        }

        public async Task<BaseResult<CategoryDto>> GetCategoryAsync(string nameCategory)
        {
            var category = await _CategoryRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == nameCategory);
            if(category == null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorGetCategory,
                    ErrorMessage = ErrorMessages.ErrorGetCategory
                };
            }
            return new BaseResult<CategoryDto>()
            {
                Data = _mapper.Map<CategoryDto>(category),
            };
        }
        public async Task<BaseResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var category = await _CategoryRepository.GetAll().FirstOrDefaultAsync(p=>p.Name==dto.Name);
            if(category == null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.CategoryIsNotFound,
                    ErrorMessage = ErrorMessages.CategoryIsNotFound
                };
            }
            try
            {
                category.Name = dto.NewName;
                category.Description = dto.Description;
                _CategoryRepository.UpdateAsync(category);
                await _CategoryRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorUpdateCategory,
                    ErrorMessage = ErrorMessages.ErrorUpdateCategory
                };
            }
            return new BaseResult<CategoryDto>()
            {
                Data = _mapper.Map<CategoryDto>(category),
            };
        }

        public async Task<BaseResult<CategoryDto>> RemoveProductInCategoryAsync(string nameCategory, string nameProduct)
        {
            var category = await _CategoryRepository.GetAll().Include(p=>p.Products).FirstOrDefaultAsync(p=>p.Name == nameCategory);
            if(category == null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.CategoryIsNotFound,
                    ErrorMessage = ErrorMessages.CategoryIsNotFound
                };
            }
            var prod = category.Products.FirstOrDefault(p=>p.Name == nameProduct);
            if(prod == null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound
                };
            }
            using (var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    category = await _unitOfWork.Categories
                        .GetAll()
                        .Include(p => p.Products)
                        .FirstOrDefaultAsync(p => p.Name == nameCategory);
                    prod = category.Products.FirstOrDefault(p => p.Name == nameProduct);

                    category.Products.Remove(prod);
                    _unitOfWork.Categories.UpdateAsync(category);
                    prod.Category = null;
                    _unitOfWork.Products.UpdateAsync(prod);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new BaseResult<CategoryDto>
                    {   
                        Data = _mapper.Map<CategoryDto>(category),
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new BaseResult<CategoryDto>()
                    {
                        ErrorMessage = ErrorMessages.ErrorDeleteProductInCategory,
                        ErrorCode = (int)ErrorCodes.ErrorDeleteProductInCategory
                    };
                }
            }
        }

        public async Task<BaseResult<CategoryDto>> AddProductInCategoryAsync(ProductInCategoryDto dto)
        {

            var category = await _CategoryRepository.GetAll().Include(p => p.Products).FirstOrDefaultAsync(p => p.Name == dto.nameCategory);
            if (category == null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.CategoryIsNotFound,
                    ErrorMessage = ErrorMessages.CategoryIsNotFound
                };
            }
            var product = await _ProductRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.nameProduct);
            if (product == null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductIsNotFound,
                    ErrorMessage = ErrorMessages.ProductIsNotFound
                };
            }
            var prod = category.Products.FirstOrDefault(p => p.Name == dto.nameProduct);
            if (prod != null)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                    ErrorMessage = ErrorMessages.ProductAlreadyExists
                };
            }
            try
            {
                category.Products.Add(product);
                _CategoryRepository.UpdateAsync(category);
                await _CategoryRepository.SaveChangesAsync();
            }
            catch
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorCode = (int)ErrorCodes.ErrorCreateProductInCategory,
                    ErrorMessage = ErrorMessages.ErrorCreateProductInCategory
                };
            }
            return new BaseResult<CategoryDto>
            {
                Data = _mapper.Map<CategoryDto>(category),
            };
        }
    }
}
