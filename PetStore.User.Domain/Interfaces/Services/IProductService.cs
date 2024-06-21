using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Services
{
    public interface IProductService
    {
        /// <summary>
        /// добавление проодукта в бд из данных другой бд
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<BaseResult<Product>> CreateProductInRabbit(Product product);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> UpdateProductInRabbit(Product dto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> DeleteProductInRabbit(string Guid);
        /// <summary>
        /// Создание продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto);
        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto);
        /// <summary>
        /// Удаление продукта по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> DeleteProductAsync(string name);
        /// <summary>
        /// Поиск продукта по названию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> GetProductAsync(string name);
        /// <summary>
        /// Считывание всех продуктов из Бд
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<ProductDto>> GetAllProductsAsync();
        /// <summary>
        /// Считывание продукта по айди
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> GetProductGuidIdAsync(string guidId);
        /// <summary>
        /// Удаление продукта по айди
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> DeleteProductGuidIdAsync(string guidId);
        /// <summary>
        /// Получение айди продукта по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResult<ProductGuidDto>> GetProductGuidAsync(string name);
    }
}
