using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IProductLineService
    {
        Task<BaseResult<ProductLineDto>> GetProductLineAsync(string GuidId);
        Task<BaseResult<ProductLineDto>> PlusProductLineAsync(ProductLineGuidDto dto);
        Task<BaseResult<ProductLineDto>> MinusProductLineAsync(string GuidId);
    }
}
