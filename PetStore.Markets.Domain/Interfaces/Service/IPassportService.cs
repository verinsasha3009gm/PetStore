using PetStore.Markets.Domain.Dto.Passport;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IPassportService
    {
        Task<CollectionResult<PassportDto>> GetPassportNameAsync(string Name, string Familien);
        Task<BaseResult<PassportDto>> GetPassportAsync(string PassportSeria, long PassportNumber);
        Task<BaseResult<PassportDto>> CreatePassportAsync(CreatePassportDto dto);
        Task<BaseResult<PassportDto>> UpdatePassportAsync(UpdatePassportDto dto);
        Task<BaseResult<PassportDto>> DeletePassportAsync(string PassportSeria,long PassportNumber);
    }
}
