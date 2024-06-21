using PetStore.Markets.Domain.Dto.Product;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IProductService
    {
        Task<BaseResult<Product>> CreateProductInRabbit(Product product);
        Task<BaseResult<ProductDto>> UpdateProductInRabbit(Product dto);
        Task<BaseResult<ProductDto>> DeleteProductInRabbit(string Guid);
        Task<BaseResult<ProductDto>> GetProductAsync(string ProductGuidId);
        Task<BaseResult<ProductGuidDto>> CreateProductAsync(ProductDto ProductDto);
        Task<BaseResult<ProductDto>> UpdateProductAsync(ProductGuidDto ProductDto);
        Task<BaseResult<ProductDto>> DeleteProductAsync(string ProductGuidId);
    }
}
