using PetStore.Library.Entity.Product;
using PetStore.Library.Results;
using PetStore.Product.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.BaseInterfaces.Services.Product
{
    public interface IProductService
    {
        public Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto);
        public Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto);
        public Task<BaseResult<ProductDto>> DeleteProductAsync(int id);
        public Task<BaseResult<ProductDto>> GetProductAsync(int id);
        public Task<CollectionResult<ProductDto>> GetAllProductsAsync(int userId);
        public Task<CollectionResult<ProductDto>> GetAllProductsAsync();
    }
}
