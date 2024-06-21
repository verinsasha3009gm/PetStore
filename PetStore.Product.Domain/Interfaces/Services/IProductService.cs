using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Создание продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto);
        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto);
        /// <summary>
        /// Удаление продукта по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<BaseResult<ProductDto>> DeleteProductAsync(string name);
        /// <summary>
        /// Поиск продукта по названию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BaseResult<ProductGuidDto>> GetProductAsync(string name);
        /// <summary>
        /// Считывание всех продуктов из Бд
        /// </summary>
        /// <returns></returns>
        public Task<CollectionResult<ProductDto>> GetAllProductsAsync();
        /// <summary>
        /// Считывание всех продуктов по выбранной категории
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Task<CollectionResult<ProductDto>> GetAllProductInCategory(string categoryName);
        /// <summary>
        /// Считывание всех продуктов по выбранному тегу
        /// </summary>
        /// <param name="tegName"></param>
        /// <returns></returns>
        public Task<CollectionResult<ProductDto>> GetAllTeg(string tegName);
    }
}
