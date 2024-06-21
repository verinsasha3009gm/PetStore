using PetStore.Products.Domain.Dto.Teg;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface ITegService
    {
        Task<CollectionResult<TegDto>> GetAllTags();
        Task<BaseResult<TegDto>> GetTegsByIdAsync(string Name);
        Task<BaseResult<TegDto>> CreateTegAsync(TegDto dto);
        Task<BaseResult<TegDto>> UpdateTegAsync(UpdateTegDto dto);
        Task<BaseResult<TegDto>> DeleteTegAsync(string Name);
    }
}
