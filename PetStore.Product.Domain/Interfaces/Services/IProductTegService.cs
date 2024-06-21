using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Dto.ProductTeg;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface IProductTegService
    {
        Task<BaseResult<ProductTegDto>> CreateProductTeg(ProductTegDto productTeg);
        Task<BaseResult<ProductTegDto>> UpdateProductTeg(UpdateProductTegDto productTeg);
        Task<BaseResult<ProductTegDto>> DeleteProductTeg(string prodName, string teg);
    }
}
