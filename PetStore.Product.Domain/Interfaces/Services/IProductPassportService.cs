using PetStore.Products.Domain.Dto.ProductPassport;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface IProductPassportService
    {
        /// <summary>
        /// Извлечение паспорта продукта
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        Task<BaseResult<ProductPassportDto>> GetProductPassportAsync(string Company, string prodPassportName);
        /// <summary>
        /// Создание паспорта продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ProductPassportDto>> CreateProductPassportAsync(ProductPassportDto dto);
        /// <summary>
        /// Обновление паспорта продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ProductPassportDto>> UpdateProductPassportAsync(UpdateProductPassportDto dto);
        /// <summary>
        /// Удаление паспорта продукта
        /// </summary>
        /// <param name="productPassportId"></param>
        /// <returns></returns>
        Task<BaseResult<ProductPassportDto>> DeleteProductPassportAsync(string Name, string Company);

        Task<BaseResult<ProductPassportDto>> GetPassportInProductAsync(string nameProduct, string nameProductPassport, string companyProductPassport);
        Task<BaseResult<ProductPassportDto>> AddPassportInProductAsync(ProductInProductPassportDto dto);
        Task<BaseResult<ProductPassportDto>> UpdatePassportInProductAsync(ProductInProductPassportDto dto);
        Task<BaseResult<ProductPassportDto>> RemovePassportInProductAsync(string nameProduct, string nameProductPassport, string companyProductPassport);
    }
}
