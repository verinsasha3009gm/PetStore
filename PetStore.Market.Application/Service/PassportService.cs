using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Domain.Dto.Passport;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Enum;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using Serilog;

namespace PetStore.Markets.Application.Service
{
    public class PassportService : IPassportService
    {
        private readonly IBaseRepository<Passport> _passportReposiory;
        private readonly IBaseRepository<Employe> _employeReposiory;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public PassportService(IBaseRepository<Passport> passportReposiory, IBaseRepository<Employe> employeReposiory
            , IMapper mapper, ILogger logger)
        {
            _passportReposiory = passportReposiory;
            _employeReposiory = employeReposiory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResult<PassportDto>> CreatePassportAsync(CreatePassportDto dto)
        {
            var passport = await _passportReposiory.GetAll().Where(p=>p.Familien == dto.Familien).FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if (passport != null)
            {
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportNotFound,
                    ErrorMessage = ErrorMessage.PassportNotFound,
                };
            }
            try
            {
                passport = new Passport()
                {
                    PlaceOfBirth = dto.PlaceOfBirth,
                    Name = dto.Name,
                    Familien = dto.Familien,
                    Issued = dto.Issued,
                    DepartmentCode = $"<<<{dto.Name}<<{dto.Familien}<<{dto.PlaceOfBirth}<<{new Random().Next(100000,999999)}",
                    PassportNumber = new Random().Next(1000,9999),
                    PassportSeria =  new Random().Next(100000, 999999).ToString(),
                };
                await _passportReposiory.CreateAsync(passport);
            }
            catch (Exception ex)
            {
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportCreateError,
                    ErrorMessage = ErrorMessage.PassportCreateError
                };
            }
            return new BaseResult<PassportDto>
            {
                Data = _mapper.Map<PassportDto>(passport)
            };
        }
        public async Task<BaseResult<PassportDto>> DeletePassportAsync(string PassportSeria, long PassportNumber)
        {
            var passport = await _passportReposiory.GetAll().Where(p => p.PassportSeria == PassportSeria).FirstOrDefaultAsync(p => p.PassportNumber == PassportNumber);
            if (passport == null)
            {
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportNotFound,
                    ErrorMessage = ErrorMessage.PassportNotFound,
                };
            }
            try
            {
                _passportReposiory.DeleteAsync(passport);
                await _passportReposiory.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportDeleteError,
                    ErrorMessage = ErrorMessage.PassportDeleteError
                };
            }
            return new BaseResult<PassportDto>
            {
                Data = _mapper.Map<PassportDto>(passport)
            };
        }
        public async Task<BaseResult<PassportDto>> GetPassportAsync(string PassportSeria, long PassportNumber)
        {
            var passport = await _passportReposiory.GetAll().Where(p => p.PassportSeria == PassportSeria).FirstOrDefaultAsync(p => p.PassportNumber == PassportNumber);
            if (passport == null)
            {
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportNotFound,
                    ErrorMessage = ErrorMessage.PassportNotFound,
                };
            }
            return new BaseResult<PassportDto>
            {
                Data = _mapper.Map<PassportDto>(passport)
            };
        }
        public async Task<CollectionResult<PassportDto>> GetPassportNameAsync(string Name, string Familien)
        {
            var passports = await _passportReposiory.GetAll()
                .Where(p => p.Familien == Familien).Where(p => p.Name == Name).ToListAsync();
            if (passports == null)
            {
                return new CollectionResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportNotFound,
                    ErrorMessage = ErrorMessage.PassportNotFound,
                };
            }
            return new CollectionResult<PassportDto>
            {
                Data = passports
                .Select(p => new PassportDto(p.PlaceOfBirth, p.Issued, p.Name, p.Familien,p.PassportSeria,p.PassportNumber)),
                Count = passports.Count()
            };
        }
        public async Task<BaseResult<PassportDto>> UpdatePassportAsync(UpdatePassportDto dto)
        {
            var passport = await _passportReposiory
                .GetAll()
                .Where(p => p.PassportSeria == dto.PassportSeria)
                .FirstOrDefaultAsync(p => p.PassportNumber == dto.PassportNumber);
            if (passport == null)
            {
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportNotFound,
                    ErrorMessage = ErrorMessage.PassportNotFound,
                };
            }
            try
            {
                passport.PlaceOfBirth = dto.PlaceOfBirth;
                passport.Name = dto.Name;
                passport.Familien = dto.Familien;
                passport.Issued = dto.Issued;
                _passportReposiory.UpdateAsync(passport);
                await _passportReposiory.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<PassportDto>
                {
                    ErrorCode = (int)ErrorCodes.PassportUpdateError,
                    ErrorMessage = ErrorMessage.PassportUpdateError
                };
            }
            return new BaseResult<PassportDto>
            {
                Data = _mapper.Map<PassportDto>(passport)
            };
        }
    }
}
