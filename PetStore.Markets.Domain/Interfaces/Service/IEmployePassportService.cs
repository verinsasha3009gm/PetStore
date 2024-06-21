using PetStore.Markets.Domain.Dto.EmployePassport;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Service
{
    public interface IEmployePassportService
    {
        Task<BaseResult<EmployePassportGuidDto>> GetEmployePassportAsync(string Email);
        Task<BaseResult<EmployePassportDto>> GetEmployePassportGuidAsync(string EmployePassportGuid);
        Task<BaseResult<EmployePassportDto>> CreateEmployePassportAsync(CreateEmployePassportDto dto);
        Task<BaseResult<EmployePassportDto>> CreateEmployePassportInEmployeAsync(EmployePassportGuidDto dto);
        Task<BaseResult<EmployePassportDto>> UpdateEmployePassportAsync(UpdateEmployePassportDto dto);
        Task<BaseResult<EmployePassportDto>> DeleteEmployePassportAsync(string Email, string Password);
    }
}
