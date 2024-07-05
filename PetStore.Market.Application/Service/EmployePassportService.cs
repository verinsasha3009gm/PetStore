using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Application.Validate;
using PetStore.Markets.Domain.Dto.EmployePassport;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Enum;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Service
{
    public class EmployePassportService : IEmployePassportService
    {
        private readonly IBaseRepository<EmployePassport> _EmployePassportRepository;
        private readonly IBaseRepository<Employe> _EmployeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        private readonly IUnitOfWork _unitOfWork;
        public EmployePassportService(IBaseRepository<EmployePassport> employePassportRepository,
            IBaseRepository<Employe> employeRepository, IMapper mapper, ILogger logger, ICacheService cacheService
            , IUnitOfWork unitOfWork)
        {
            _EmployePassportRepository = employePassportRepository;
            _EmployeRepository = employeRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResult<EmployePassportDto>> CreateEmployePassportAsync(CreateEmployePassportDto dto)
        {
            var emp = await _EmployeRepository.GetAll().Include(p=>p.EmployePassport).FirstOrDefaultAsync(p=>p.Email == dto.Email);
            if(emp == null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var empPassport = emp.EmployePassport;
            if(empPassport != null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportAlreadyExists,
                    ErrorMessage = ErrorMessage.EmployePassportAlreadyExists
                };
            }
            try
            {
                empPassport = new EmployePassport()
                {
                    Post = dto.Post,
                    Salary = dto.Salary,
                    Experience = dto.Expirience,
                    GuidId= Guid.NewGuid().ToString(),
                    EnployeId = emp.Id,
                };
                empPassport = await _EmployePassportRepository.CreateAsync(empPassport);
                _cacheService.Set(empPassport.GuidId, empPassport);

                emp.EmployePassportId = empPassport.Id;
                _EmployeRepository.UpdateAsync(emp);
                await _EmployeRepository.SaveChangesAsync();
                _cacheService.Set(emp.GuidId, emp);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<EmployePassportDto>
                {
                    ErrorMessage = ErrorMessage.EmployePassportCreateError,
                    ErrorCode = (int)ErrorCodes.EmployePassportCreateError
                };
            }
            return new BaseResult<EmployePassportDto>
            {
                Data = _mapper.Map<EmployePassportDto>(empPassport)
            };
        }
        public async Task<BaseResult<EmployePassportDto>> CreateEmployePassportInEmployeAsync(EmployePassportGuidDto dto)
        {
            var emp = await _EmployeRepository.GetAll().Include(p => p.EmployePassport).FirstOrDefaultAsync(p => p.GuidId == dto.GuidId);
            if (emp == null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var empPassport = emp.EmployePassport;
            if (empPassport != null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            using(var trasaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    empPassport = new EmployePassport()
                    {
                        Post = dto.Post,
                        Salary = dto.Salary,
                        Experience = dto.Expirience,
                        GuidId =Guid.NewGuid().ToString(),
                        EnployeId = emp.Id,
                    };
                    empPassport = await _unitOfWork.EmployesPassports.CreateAsync(empPassport);

                    emp.EmployePassportId = empPassport.Id;
                    _unitOfWork.Employes.UpdateAsync(emp);
                    await _unitOfWork.Employes.SaveChangesAsync();

                    await trasaction.CommitAsync();
                    _cacheService.Set(emp.GuidId, emp);

                    return new BaseResult<EmployePassportDto>
                    {
                        Data = _mapper.Map<EmployePassportDto>(empPassport)
                    };
                }
                catch (Exception ex)
                {
                    await trasaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<EmployePassportDto>
                    {
                        ErrorMessage = ErrorMessage.EmployePassportCreateError,
                        ErrorCode = (int)ErrorCodes.EmployePassportCreateError
                    };
                }
            }
            
        }

        public async Task<BaseResult<EmployePassportDto>> DeleteEmployePassportAsync(string Email, string Password)
        {
            var emp = await _EmployeRepository.GetAll().Include(p=>p.EmployePassport).FirstOrDefaultAsync(p => p.Email == Email);
            if(emp == null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            if (!PasswordValidate.IsVerifyPassword(emp.Password, Password))
            {
                return new BaseResult<EmployePassportDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsNotValid,
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid
                };
            }
            var empPassport = emp.EmployePassport;
            if(empPassport == null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            try
            {
                _EmployePassportRepository.DeleteAsync(empPassport);
                await _EmployePassportRepository.SaveChangesAsync();
                //_cacheService.Delete<EmployePassport>(empPassport.GuidId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<EmployePassportDto>
                {
                    ErrorMessage = ErrorMessage.EmployePassportDeleteError,
                    ErrorCode = (int)ErrorCodes.EmployePassportDeleteError
                };
            }
            return new BaseResult<EmployePassportDto>
            {
                Data = _mapper.Map<EmployePassportDto>(empPassport)
            };
        }
        public async Task<BaseResult<EmployePassportGuidDto>> GetEmployePassportAsync(string Email)
        {
            var emp = await _EmployeRepository.GetAll().Include(p=>p.EmployePassport).FirstOrDefaultAsync(p=>p.Email == Email);
            if (emp == null)
            {
                return new BaseResult<EmployePassportGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var empPassport = emp.EmployePassport;
            if(empPassport == null)
            {
                return new BaseResult<EmployePassportGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            _cacheService.Refrech<EmployePassport>(empPassport.GuidId);
            return new BaseResult<EmployePassportGuidDto>
            {
                Data = _mapper.Map<EmployePassportGuidDto>(empPassport)
            };
        }

        public async Task<BaseResult<EmployePassportDto>> GetEmployePassportGuidAsync(string EmployePassportGuid)
        {
            var empPassp = _cacheService.Get<EmployePassport>(EmployePassportGuid);
            if(empPassp == null)
            {
                empPassp =await _EmployePassportRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == EmployePassportGuid);
                if (empPassp == null)
                {
                    return new BaseResult<EmployePassportDto>
                    {
                        ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                        ErrorMessage = ErrorMessage.EmployePassportNotFound
                    };
                }
            }
            _cacheService.Refrech<EmployePassport>(empPassp.GuidId);
            return new BaseResult<EmployePassportDto>
            {
                Data = _mapper.Map<EmployePassportDto>(empPassp)
            };
        }

        public async Task<BaseResult<EmployePassportDto>> UpdateEmployePassportAsync(UpdateEmployePassportDto dto)
        {
            var emp = await _EmployeRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == dto.Email);
            if (emp == null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var empPassport = emp.EmployePassport;
            if(empPassport == null)
            {
                return new BaseResult<EmployePassportDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            try
            {
                empPassport.Salary = dto.Salary;
                empPassport.Experience = dto.Expirience;
                empPassport.Post = dto.Post;
                _EmployePassportRepository.UpdateAsync(empPassport);
                await _EmployePassportRepository.SaveChangesAsync();
                _cacheService.Set(empPassport.GuidId, empPassport);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<EmployePassportDto>
                {
                    ErrorMessage = ErrorMessage.EmployePassportUpdateError,
                    ErrorCode = (int)ErrorCodes.EmployePassportUpdateError
                };
            }
            return new BaseResult<EmployePassportDto>()
            {
                Data = _mapper.Map<EmployePassportDto>(empPassport)
            };
        }
    }
}
