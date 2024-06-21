using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PetStore.Users.Application.Resources;
using PetStore.Users.Domain.Enum;
using Serilog;

namespace PetStore.Users.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IBaseRepository<Address> _AddressRepository;
        private readonly IBaseRepository<User> _UserRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        public AddressService(IBaseRepository<Address> AddressRepository, IBaseRepository<User> UserRepository
            ,IMapper mapper,ILogger logger, ICacheService cacheService)
        {
            _AddressRepository = AddressRepository;
            _UserRepository = UserRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<BaseResult<Address>> AddAddressInRabbit(Address address)
        {
            var addressGuid = await _AddressRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == address.GuidId);
            if (addressGuid != null)
            {
                return new BaseResult<Address>
                {
                    ErrorCode = (int)ErrorCodes.AddressAlreadyExists,
                    ErrorMessage = ErrorMessage.AddressAlreadyExists
                };
            }
            try
            {
                address.Id = 0;
                await _AddressRepository.CreateAsync(address);
                _cacheService.Set(address.GuidId, address);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<Address>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<Address>
            {
                Data = address
            };
        }
        public async Task<BaseResult<Address>> RemoveAddressInRabbit(string guidId)
        {
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
            if (address == null)
            {
                return new BaseResult<Address>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            try
            {
                _AddressRepository.DeleteAsync(address);
                await _AddressRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<Address>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<Address>
            {
                Data = address
            };
        }
        public async Task<BaseResult<AddressDto>> AddAddressInUserAsync(AddressInUserDto address)
        {
            var user = await _UserRepository.GetAll().Include(p => p.Addresses).FirstOrDefaultAsync(p => p.Login == address.userLogin);
            if (user == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode= (int)ErrorCodes.UserNotFound,
                    ErrorMessage= ErrorMessage.UserNotFound
                };
            }
            var addressNew = new Address()
            {
                Region = address.Region,
                City = address.City,
                Street = address.Street,
                Country = address.Country,
                UserId = user.Id,
                GuidId = Guid.NewGuid().ToString(),
            };
            var addressN =  user.Addresses.Where(p => p.Country == address.Country && p.Region == address.Region && p.City == address.City)
                .FirstOrDefault( p=> p.Street == address.Street);
            if(addressN != null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressAlreadyExists,
                    ErrorMessage = ErrorMessage.AddressAlreadyExists
                };
            }
            try
            {
                await _AddressRepository.CreateAsync(addressNew);
                user.Addresses.Add(addressNew);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                _cacheService.Set(addressNew.GuidId, addressNew);
            }
            catch(Exception ex) 
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<AddressDto>
            {
                Data = _mapper.Map<AddressDto>(addressNew),
            };
        }
        public async Task<BaseResult<AddressDto>> GetAddressInUserAsync(string guidId, string userLogin)
        {
            var user = await _UserRepository.GetAll().Include(p=>p.Addresses).FirstOrDefaultAsync(p=>p.Login == userLogin);
            if(user == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var address = _cacheService.Get<Address>(guidId);
            if (address != null)
            {
                if(address.UserId == user.Id)
                {
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address),
                    };
                }
            }
            address = user.Addresses.FirstOrDefault(p=>p.GuidId == guidId);
            if(address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            _cacheService.Refrech<Address>(address.GuidId);
            _cacheService.Refrech<User>(user.GuidId);
            return new BaseResult<AddressDto>
            {
                Data = _mapper.Map<AddressDto>(address),
            };
        }
        public async Task<CollectionResult<AddressDto>> GetAllAddressesInUserAsync(string userLogin)
        {
            var userAddresses = await _UserRepository.GetAll().Include(p=>p.Addresses).FirstOrDefaultAsync(p=>p.Login == userLogin);
            if(userAddresses == null)
            {
                return new()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var Addresses = userAddresses.Addresses.Select(p=>new AddressDto(p.Region,p.Country,p.City,p.Street,p.GuidId));
            if (Addresses == null)
            {
                return new()
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            return new()
            {
                Data = Addresses
            };
        }
        public async Task<BaseResult<AddressDto>> RemoveAddressInUserAsync(string guidId, string userLogin)
        {
            var user = await _UserRepository.GetAll().Include(p => p.Addresses).FirstOrDefaultAsync(p => p.Login == userLogin);
            if (user == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var  address = user.Addresses.FirstOrDefault(p => p.GuidId == guidId);
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            try
            {
                _AddressRepository.DeleteAsync(address);
                await _AddressRepository.SaveChangesAsync();
                //_cacheService.Delete<Address>(guidId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,ex.Message);
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }

            return new BaseResult<AddressDto>
            {
                Data = _mapper.Map<AddressDto>(address)
            };
        }
    }
}
