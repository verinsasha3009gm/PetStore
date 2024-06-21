using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Application.Validate;
using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Dto.Employe;
using PetStore.Markets.Domain.Dto.EmployePassport;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Enum;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PetStore.Markets.Application.Service
{
    public class EmployeService : IEmployeService
    {
        private readonly IBaseRepository<Employe> _EmployeRepository;
        private readonly IBaseRepository<Token> _TokenRepository;
        private readonly IEmployePassportService _EmployePassportService;
        private readonly ITokenService _TokenService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public EmployeService(IBaseRepository<Employe> employeRepository, IBaseRepository<Token> tokenRepository
            , ITokenService tokenService,  IEmployePassportService passportService,IMapper mapper,ILogger logger,ICacheService cacheService)
        {
            _EmployeRepository = employeRepository;
            _TokenRepository = tokenRepository;
            _TokenService = tokenService;
            _EmployePassportService = passportService;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<BaseResult<EmployeDto>> DeleteEmployeAsync(string Email, string Password)
        {
            var employe = await _EmployeRepository.GetAll().FirstOrDefaultAsync(p => p.Email == Email);
            if(employe == null)
            {
                return new BaseResult<EmployeDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            if(!PasswordValidate.IsVerifyPassword(employe.Password, Password))
            {
                return new BaseResult<EmployeDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
                };
            }
            try
            {
                _EmployeRepository.DeleteAsync(employe);
                await _EmployeRepository.SaveChangesAsync();
                //_cacheService.Delete<Employe>(employe.GuidId);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<EmployeDto>
                {
                    ErrorMessage = ErrorMessage.EmployeDeleteError,
                    ErrorCode = (int)ErrorCodes.EmployeDeleteError
                };
            }
            return new BaseResult<EmployeDto>
            {
                Data = _mapper.Map<EmployeDto>(employe)
            };
        }
        public async Task<CollectionResult<EmployeDto>> GetAllEmployesAsync()
        {
            var Employes = await _EmployeRepository.GetAll().Select(p=>new EmployeDto(p.Name,p.Gender)).ToListAsync();
            if (Employes == null)
            {
                return new CollectionResult<EmployeDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            return new CollectionResult<EmployeDto>
            {
                Data = Employes,
                Count = Employes.Count()
            };
        }
        public async Task<BaseResult<EmployeDto>> GetEmployeAsync(string Email)
        {
            var employe = await _EmployeRepository.GetAll().FirstOrDefaultAsync(p => p.Email == Email);
            if(employe == null)
            {
                return new BaseResult<EmployeDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            //_cacheService.Refrech<Employe>(employe.GuidId);
            return new BaseResult<EmployeDto>
            {
                Data = _mapper.Map<EmployeDto>(employe)
            };
        }
        public async Task<BaseResult<EmployeGuidDto>> GetEmployeGuidIdAsync(string Email, string password)
        {
            var emp = await _EmployeRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == Email);
            if(emp == null)
            {
                return new BaseResult<EmployeGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            if(!PasswordValidate.IsVerifyPassword( emp.Password,password))
            {
                return new()
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
                };
            }
            //_cacheService.Refrech<Employe>(emp.GuidId);
            return new()
            {
                Data = _mapper.Map<EmployeGuidDto>(emp)
            };
        }
        public async Task<BaseResult<TokenDto>> LoginEmployeAsync(LoginEmployeDto dto)
        {
            var employe = await _EmployeRepository.GetAll().Include(p=>p.EmployePassport).FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (employe == null)
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.EmployeNotFound,
                    ErrorCode = (int)ErrorCodes.EmployeNotFound
                };
            }
            if (!PasswordValidate.IsVerifyPassword(employe.Password, dto.Password))
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsNotValid,
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid
                };
            }
            var employeToken = await _TokenRepository.GetAll().FirstOrDefaultAsync(p => p.EmployeId == employe.Id);

            var employeRole = employe.EmployePassport.Post;
            if (employeRole == null)
            {
                employe.EmployePassport.Post = "Сashier";
                _EmployeRepository.UpdateAsync(employe);
                await _EmployeRepository.SaveChangesAsync();
            }
            _cacheService.Refrech<Employe>(employe.GuidId);

            employeRole = employe.EmployePassport.Post;
            var claimRole = new Claim(ClaimTypes.Role, employeRole);
            var claim = new Claim(ClaimTypes.Name, employe.Email);
            var claims = new List<Claim> { claim, claimRole };
            var accessToken = _TokenService.GenerateAccessToken(claims);
            var refreshToken = _TokenService.GenerateRefreshToken();
            if (employeToken == null)
            {
                employeToken = new Token()
                {
                    EmployeId = employe.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                };
                await _TokenRepository.CreateAsync(employeToken);
            }
            else
            {
                employeToken.RefreshToken = refreshToken;
                employeToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                _TokenRepository.UpdateAsync(employeToken);
                await _TokenRepository.SaveChangesAsync();
            }
            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    RefreshToken = refreshToken,
                    AccessToken = accessToken
                }
            };
        }
        public async Task<BaseResult<EmployeDto>> RegistrationEmployeAsync(RegistrationEmployeDto dto)
        {
            if (dto.Password != dto.AlreadyPassword)
            {
                return new BaseResult<EmployeDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsNotValid,
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid
                };
            }

            var employe = await _EmployeRepository.GetAll().FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (employe != null)
            {
                return new BaseResult<EmployeDto>()
                {
                    ErrorMessage = ErrorMessage.EmployeAlreadyExists,
                    ErrorCode = (int)ErrorCodes.EmployeAlreadyExists
                };
            }
            var hashUserPassword = PasswordValidate.HashPassword(dto.Password);
            employe = new Employe()
            {
                Gender = dto.Gender,
                Name = dto.Name,
                Email = dto.Email,
                Password = hashUserPassword,
                GuidId = Guid.NewGuid().ToString(),

            };
            try
            {
                await _EmployeRepository.CreateAsync(employe);
                _cacheService.Set(employe.GuidId.ToString(), employe);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<EmployeDto>
                {
                    ErrorMessage = ErrorMessage.EmployeCreateError,
                    ErrorCode = (int)ErrorCodes.EmployeCreateError
                };
            }
            var employePassport = new EmployePassport()
            {
                Post= dto.Post,
                Experience = dto.Expirience,
                Salary = dto.Salary,
                GuidId = employe.GuidId,
            };
            try
            {
                var createPassportDto = new EmployePassportGuidDto(dto.Post,dto.Expirience, dto.Salary,employePassport.GuidId);
                var p =  await _EmployePassportService.CreateEmployePassportInEmployeAsync(createPassportDto);
                if (!p.IsSucces)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                _EmployeRepository.DeleteAsync(employe);
                _logger.Error(ex, ex.Message);
                return new BaseResult<EmployeDto>()
                {
                    ErrorMessage = ErrorMessage.EmployePassportCreateError,
                    ErrorCode = (int)ErrorCodes.EmployePassportCreateError
                };
            }
            return new BaseResult<EmployeDto>()
            {
                Data = _mapper.Map<EmployeDto>(employe)
            };
        }
        public async Task<BaseResult<EmployeDto>> UpdateEmployeAsync(UpdateEmployeDto dto)
        {
            var emp = await _EmployeRepository.GetAll().FirstOrDefaultAsync(p=>p.Email == dto.Email);
            if(emp == null)
            {
                return new BaseResult<EmployeDto>()
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            if(!PasswordValidate.IsVerifyPassword(emp.Password,dto.Password))
            {
                return new BaseResult<EmployeDto>
                {
                    ErrorCode = (int)ErrorCodes.PasswordIsNotValid,
                    ErrorMessage = ErrorMessage.PasswordIsNotValid
                };
            }
            try
            {
                emp.Name = dto.NewNameEmploye;
                emp.Gender = dto.NewGender;
                _EmployeRepository.UpdateAsync(emp);
                await _EmployeRepository.SaveChangesAsync();
                _cacheService.Set<Employe>(emp.GuidId, emp);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<EmployeDto>
                {
                    ErrorMessage = ErrorMessage.EmployeUpdateError,
                    ErrorCode = (int)ErrorCodes.EmployeUpdateError
                };
            }
            return new BaseResult<EmployeDto>()
            {
                Data = _mapper.Map<EmployeDto>(emp)
            };
        }
    }
}
