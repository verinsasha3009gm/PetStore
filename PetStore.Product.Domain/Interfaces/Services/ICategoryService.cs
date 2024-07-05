using PetStore.Products.Domain.Result;
using PetStore.Products.Domain.Dto.Category;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Извлечение всех категорий
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<CategoryDto>> GetAllCategories();
        /// <summary>
        /// Извлечение категории по имени
        /// </summary>
        /// <param name="nameCategory"></param>
        /// <returns></returns>
        Task<BaseResult<CategoryDto>> GetCategoryAsync(string nameCategory);
        /// <summary>
        /// Создание новой категории
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<CategoryDto>> CreateCategoryAsync(CreateCategoryDto dto);
        /// <summary>
        /// Обновление категории
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<BaseResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto category);
        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="nameCategory"></param>
        /// <returns></returns>
        Task<BaseResult<CategoryDto>> DeleteCategoryAsync(string nameCategory);
        /// <summary>
        /// Добавление продукта в выбранную категорию
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<CategoryDto>> AddProductInCategoryAsync(ProductInCategoryDto dto);
        /// <summary>
        /// Удаление продукта из выбранной категории
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<CategoryDto>> RemoveProductInCategoryAsync(string nameCategory, string nameProduct);
    }
}
