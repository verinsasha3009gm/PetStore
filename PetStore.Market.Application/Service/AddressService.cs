using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Markets.Producer.Interfaces;
using PetStore.Markets.Application.Resources;
using PetStore.Markets.Domain.Dto.Address;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Enum;
using PetStore.Markets.Domain.Interfaces.Repositories;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;
using Serilog;
using Microsoft.Extensions.Options;
using PetStore.Markets.Domain.Settings;

namespace PetStore.Markets.Application.Service
{
    public class AddressService : IAddressService
    {
        private readonly IBaseRepository<User> _UserRepository;
        private readonly IBaseRepository<Address> _AddressRepository;
        private readonly IBaseRepository<Employe> _EmployeRepository;
        private readonly IBaseRepository<EmployePassport> _EmployePassportRepository;
        private readonly IBaseRepository<Market> _MarketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;
        private readonly IMessageProducer _messageProducer;
        private readonly IOptions<RabbitMqSettings> _rabbitOptions;
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IBaseRepository<Address> AddressRepository, IBaseRepository<User> userRepository,
            IBaseRepository<Employe> empRepository, IBaseRepository<EmployePassport> empPassportRepository,
            IBaseRepository<Market> marketRepository,IMapper mapper, ILogger logger, ICacheService cacheService,
            IMessageProducer messageProducer,IOptions<RabbitMqSettings> rabbitOptions, IUnitOfWork unitOfWork)
        {
            _AddressRepository = AddressRepository;
            _UserRepository = userRepository;
            _EmployeRepository = empRepository;
            _EmployePassportRepository = empPassportRepository;
            _MarketRepository = marketRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
            _messageProducer = messageProducer;
            _rabbitOptions = rabbitOptions;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResult<AddressGuidDto>> CreateAddressAsync(AddressDto dto)
        {
            var address = await _AddressRepository.GetAll()
                .Where(p=>p.City == dto.City)
                .Where(p=>p.Country == dto.Country)
                .Where(p=>p.Region == dto.Region)
                .FirstOrDefaultAsync(p=>p.Street == dto.Street);
            if(address != null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressAlreadyExists,
                    ErrorMessage = ErrorMessage.AddressAlreadyExists
                };
            }
            address = new Address()
            {
                City = dto.City,
                Country = dto.Country,
                Region = dto.Region,
                Street = dto.Street,
                GuidId = Guid.NewGuid().ToString(),
            }; 
            try
            {
                await _AddressRepository.CreateAsync(address);
                _cacheService.Set(address.GuidId, address);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressCreateError,
                    ErrorMessage = ErrorMessage.AddressCreateError,
                };
            }
            _messageProducer
                .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address),
            };
        }
        public async Task<BaseResult<AddressDto>> GetAddressAsync(string guidId)
        {
            var address = _cacheService.Get<Address>(guidId);
            if(address == null)
            {
                address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
                if (address == null)
                {
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressNotFound,
                        ErrorMessage = ErrorMessage.AddressNotFound
                    };
                }
            }
            return new BaseResult<AddressDto>
            {
                Data = _mapper.Map<AddressDto>(address)
            };
        }
        public async Task<BaseResult<AddressDto>> UpdateAddressAsync(AddressGuidDto dto)
        {
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.GuidId);
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
                address.Street = dto.Street;
                address.City = dto.City;
                address.Region = dto.Region;
                address.Country = dto.Country;
                _AddressRepository.UpdateAsync(address);
                await _AddressRepository.SaveChangesAsync();
                _cacheService.Set<Address>(dto.GuidId, address);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressUpdateError,
                    ErrorMessage = ErrorMessage.AddressUpdateError
                };
            }
            return new BaseResult<AddressDto>
            {
                Data = _mapper.Map<AddressDto>(address)
            };
        }
        public async Task<BaseResult<AddressDto>> DeleteAddressAsync(string guidId)
        {
            var  address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
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
                //Cache.Delete
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressDeleteError,
                    ErrorMessage = ErrorMessage.AddressDeleteError
                };
            }
            return new BaseResult<AddressDto>
            {
                Data = _mapper.Map<AddressDto>(address),
            };
        }

        public async Task<BaseResult<AddressDto>> AddAddressInEmployePassportAsync(AddressEmployePassportDto dto)
        {
            var employe = await _EmployeRepository.GetAll()
                .Include(p=>p.EmployePassport)
                .FirstOrDefaultAsync(p => p.Name == dto.EmployeName);
            if (employe == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var employePassport = employe.EmployePassport;
            if (employePassport == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            var address = await _AddressRepository.GetAll()
                .Where(p => p.City == dto.City)
                .Where(p => p.Region == dto.Region)
                .Where(p => p.Street == dto.Street)
                .FirstOrDefaultAsync(p => p.Country == dto.Country);
            if(address != null)
            {
                address.EmployePassportId = employe.Id;
                _AddressRepository.UpdateAsync(address);
                await _AddressRepository.SaveChangesAsync();
                _cacheService.Set(address.GuidId, address);
                return new BaseResult<AddressDto>
                {
                    Data = new AddressDto(address.Country, address.Region, address.City, address.Street)
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    address = new Address
                    {
                        Street = dto.Street,
                        City = dto.City,
                        Region = dto.Region,
                        Country = dto.Country,
                        EmployePassportId = employePassport.Id,
                        GuidId = Guid.NewGuid().ToString(),
                    };
                    await _unitOfWork.Addresses.CreateAsync(address);
                    _cacheService.Set(address.GuidId, address);
                    employePassport.Address = address;
                    _unitOfWork.EmployesPassports.UpdateAsync(employePassport);
                    await _unitOfWork.EmployesPassports.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressCreateError,
                        ErrorMessage = ErrorMessage.AddressCreateError,
                    };
                }
            }
        }
        public async Task<BaseResult<AddressDto>> AddAddressInEmployePassportGuidAsync(AddressEmployePassportGuidDto dto)
        {
            var employePassport = await _EmployePassportRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.EmployePassportId);
            if (employePassport == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            
            
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.AddressGuidId);
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            using (var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    address.EmployePassportId = employePassport.Id;
                    _unitOfWork.Addresses.UpdateAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();
                    _cacheService.Set(address.GuidId, address);

                    employePassport.Address= address;
                    _unitOfWork.EmployesPassports.UpdateAsync(employePassport);
                    await _unitOfWork.EmployesPassports.SaveChangesAsync();
                    _cacheService.Set(employePassport.GuidId, employePassport);

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey
                        , _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressCreateError,
                        ErrorMessage = ErrorMessage.AddressCreateError,
                    };
                }
            }
        }

        public async Task<BaseResult<AddressDto>> AddAddressInMarketAsync(AddressMarketDto dto)
        {
            var market = await _MarketRepository.GetAll().FirstOrDefaultAsync(p => p.Name == dto.MarketName);
            if (market == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            var address = await _AddressRepository.GetAll()
                .Where(p => p.City == dto.City)
                .Where(p => p.Region == dto.Region)
                .Where(p => p.Street == dto.Street)
                .FirstOrDefaultAsync(p => p.Country == dto.Country);
            if (address != null) 
            {
                using(var transaction = await _unitOfWork.BeginTransitionAsync())
                {
                    try
                    {
                        address.MarketId = market.Id;
                        _unitOfWork.Addresses.UpdateAsync(address);
                        await _unitOfWork.Addresses.SaveChangesAsync();
                        market.Adress = address;
                        _unitOfWork.Markets.UpdateAsync(market);
                        await _unitOfWork.Markets.SaveChangesAsync();

                        await transaction.CommitAsync();

                        _cacheService.Set(address.GuidId,address);
                        _messageProducer
                            .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                        return new BaseResult<AddressDto>
                        {
                            Data = new AddressDto(address.Country, address.Region, address.City, address.Street)
                        };
                    }
                    catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.Error(ex, ex.Message);
                        return new BaseResult<AddressDto>
                        {
                            ErrorCode = (int)ErrorCodes.AddressCreateError,
                            ErrorMessage = ErrorMessage.AddressCreateError,
                        };
                    }
                }
            }
            using (var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    address = new Address
                    {
                        Street = dto.Street,
                        City = dto.City,
                        Region = dto.Region,
                        Country = dto.Country,
                        MarketId = market.Id,
                        GuidId = Guid.NewGuid().ToString(),
                    };
                    await _unitOfWork.Addresses.CreateAsync(address);
                    _cacheService.Set(address.GuidId, address); 
                    market.Adress = address;
                    _unitOfWork.Markets.UpdateAsync(market);
                    await _unitOfWork.Markets.SaveChangesAsync();
                    _cacheService.Set(market.GuidId, market);

                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    await transaction.CommitAsync();
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressCreateError,
                        ErrorMessage = ErrorMessage.AddressCreateError,
                    };
                }
            }
        }
        public async Task<BaseResult<AddressDto>> AddAddressInMarketGuidAsync(AddressMarketGuidDto dto)
        {
            var market = await _MarketRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.marketGuidId);
            if (market == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
           
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.addressGuidId);
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    address.MarketId = market.Id;
                    _unitOfWork.Addresses.UpdateAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();
                    _cacheService.Set(address.GuidId,address);

                    market.AdressId = address.Id;
                    _unitOfWork.Markets.UpdateAsync(market);
                    await _unitOfWork.Markets.SaveChangesAsync();
                    _cacheService.Set(market.GuidId, market);

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressCreateError,
                        ErrorMessage = ErrorMessage.AddressCreateError,
                    };
                }
            }
        }

        public async Task<BaseResult<AddressGuidDto>> AddAddressInUserAsync(AddressUserDto dto)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.Login == dto.Login);
            if (user == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var address = await _AddressRepository.GetAll()
                .Where(p => p.City == dto.City)
                .Where(p=>p.Region == dto.Region)
                .Where(p=>p.Street == dto.Street)
                .FirstOrDefaultAsync(p => p.Country == dto.Country);
            if (address != null)
            {
                address.UserId = user.Id;
                _AddressRepository.UpdateAsync(address);
                await _AddressRepository.SaveChangesAsync();
                _cacheService.Set(address.GuidId , address);
                return new BaseResult<AddressGuidDto>
                {
                    Data = _mapper.Map<AddressGuidDto>(address)
                };
            }
            address = new Address
            {
                 Street = dto.Street,
                 City = dto.City,
                 Region = dto.Region,
                 Country = dto.Country,
                 UserId = user.Id,
                 GuidId = Guid.NewGuid().ToString(),
            };
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    await _unitOfWork.Addresses.CreateAsync(address);
                    _cacheService.Set(address.GuidId, address);

                    user.Adresses.Add(address);
                    _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.Users.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressGuidDto>
                    {
                        Data = _mapper.Map<AddressGuidDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressGuidDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressCreateError,
                        ErrorMessage = ErrorMessage.AddressCreateError,
                    };
                }
            }
        }
        public async Task<BaseResult<AddressGuidDto>> AddAddressInUserGuidAsync(AddressUserGuidDto dto)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.userId);
            if (user == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == dto.addressId);
            if (address == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    address.UserId = user.Id;
                    _unitOfWork.Addresses.UpdateAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();
                    _cacheService.Set(address.GuidId,address);

                    user.Adresses.Add(address);
                    _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.Users.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.POST), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressGuidDto>
                    {
                        Data = _mapper.Map<AddressGuidDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressGuidDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressCreateError,
                        ErrorMessage = ErrorMessage.AddressCreateError,
                    };
                }
            }
        }

        public async Task<CollectionResult<AddressGuidDto>> GetAddressesGuidInUserAsync(string guidId)
        {
            
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == guidId);
            if (user == null)
            {
                return new CollectionResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            if(user.Adresses == null)
            {
                return new CollectionResult<AddressGuidDto>
                {
                    Data = new List<AddressGuidDto> { },
                    Count = 0
                };
            }
            var addresses = user.Adresses.Select(p => new AddressGuidDto(p.GuidId, p.Country, p.Region, p.City, p.Street));
            if (addresses == null)
            {
                return new CollectionResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            return new CollectionResult<AddressGuidDto>
            {
                Data = addresses,
                Count = addresses.Count()
            };
        }
        public async Task<CollectionResult<AddressDto>> GetAddressesInUserAsync(string Login, string Passport)
        {
            var user = await _UserRepository.GetAll().Include(p => p.Adresses).FirstOrDefaultAsync(p => p.Login == Login);
            if (user == null)
            {
                return new CollectionResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound,
                };
            }
            var addresses = user.Adresses.Select(p => new AddressDto( p.Country, p.Region, p.City, p.Street));
            if (addresses == null)
            {
                return new CollectionResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            
            return new CollectionResult<AddressDto>
            {
                Data = addresses,
                Count = addresses.Count()
            };
        }

        public async Task<BaseResult<AddressGuidDto>> GetEmployePassportAddressAsync(string EmployeName)
        {
            var emp = await _EmployeRepository.GetAll().Include(p=>p.EmployePassport).ThenInclude(p=>p.Address).FirstOrDefaultAsync(p => p.Name == EmployeName);
            if (emp == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            if(emp.EmployePassport == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            if (emp.EmployePassport.Address == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            var address = emp.EmployePassport.Address;

            _cacheService.Refrech<Address>(address.GuidId);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address)
            };
        }
        public async Task<BaseResult<AddressGuidDto>> GetEmployePassportGuidAddressAsync(string EmployePassportGuidId)
        {
            var emp = await _EmployePassportRepository.GetAll().Include(p=>p.Address).FirstOrDefaultAsync(p=>p.GuidId == EmployePassportGuidId);
            if(emp == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var address = emp.Address;
            if(address == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            _cacheService.Refrech<Address>(address.GuidId);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address)
            };
        }

        public async Task<BaseResult<AddressGuidDto>> GetMarketAddressAsync(string MarketName)
        {
            var market = await _MarketRepository.GetAll().Include(p=>p.Adress).FirstOrDefaultAsync(p => p.Name == MarketName);
            if(market == null)
            {
                return new BaseResult<AddressGuidDto>()
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound,
                };
            }
            var address = market.Adress;
            if(address == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            _cacheService.Refrech<Address>(address.GuidId);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address)
            };
        }
        public async Task<BaseResult<AddressGuidDto>> GetMarketGuidAddressAsync(string MarketGuidId)
        {
            var market = await _MarketRepository.GetAll().Include(p=>p.Adress).FirstOrDefaultAsync(p=>p.GuidId == MarketGuidId);
            if(market == null)
            {
                return new BaseResult<AddressGuidDto>()
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            var address = market.Adress;
            if(address == null)
            {
                return new BaseResult<AddressGuidDto>()
                {
                    ErrorMessage = ErrorMessage.AddressNotFound,
                    ErrorCode = (int)ErrorCodes.AddressNotFound
                };
            }
            _cacheService.Refrech<Address>(address.GuidId);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address)
            };
        }

        public async Task<BaseResult<AddressGuidDto>> GetUserAddressAsync(string Login, string addressGuidId)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.Login == Login);
            if (user == null)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var address = _cacheService.Get<Address>(addressGuidId);
            if(address == null)
            {
                address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == addressGuidId);
                if (address == null)
                {
                    return new BaseResult<AddressGuidDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressNotFound,
                        ErrorMessage = ErrorMessage.AddressNotFound
                    };
                }
            }
            if(address.UserId != user.Id)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressAlreadyExists,
                    ErrorMessage = ErrorMessage.AddressAlreadyExists
                };
            }
            _cacheService.Refrech<Address>(address.GuidId);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address)
            };
        }
        public async Task<BaseResult<AddressGuidDto>> GetUserGuidAddressAsync(string userIdGuid, string addressGuidId)
        {
            var user = _cacheService.Get<User>(userIdGuid);
            if(user == null)
            {
                user = await _UserRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == userIdGuid);
                if (user == null)
                {
                    return new BaseResult<AddressGuidDto>
                    {
                        ErrorCode = (int)ErrorCodes.UserNotFound,
                        ErrorMessage = ErrorMessage.UserNotFound
                    };
                }
            }
            var address = _cacheService.Get<Address>(addressGuidId);
            if (address == null)
            {
                address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == addressGuidId);
                if (address == null)
                {
                    return new BaseResult<AddressGuidDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressNotFound,
                        ErrorMessage = ErrorMessage.AddressNotFound
                    };
                }
            }
            if (address.UserId != user.Id)
            {
                return new BaseResult<AddressGuidDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressAlreadyExists,
                    ErrorMessage = ErrorMessage.AddressAlreadyExists
                };
            }
            _cacheService.Refrech<Address>(address.GuidId);
            return new BaseResult<AddressGuidDto>
            {
                Data = _mapper.Map<AddressGuidDto>(address)
            };
        }

        public async Task<BaseResult<AddressDto>> RemoveAddressInEmployePassportAsync(string Name)
        {
            var emp = await _EmployeRepository.GetAll().Include(p=>p.EmployePassport).ThenInclude(p=>p.Address).FirstOrDefaultAsync(p => p.Name == Name);
            if(emp == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployeNotFound,
                    ErrorMessage = ErrorMessage.EmployeNotFound
                };
            }
            var empPassp = emp.EmployePassport;
            if (empPassp == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            var address = empPassp.Address;
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOfWork.Addresses.DeleteAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();

                    empPassp.Address = null;
                    _unitOfWork.EmployesPassports.UpdateAsync(empPassp);
                    await _unitOfWork.EmployesPassports.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey
                        , _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressDeleteError,
                        ErrorMessage = ErrorMessage.AddressDeleteError
                    };
                }
            }
        }
        public async Task<BaseResult<AddressDto>> RemoveAddressInEmployePassportGuidAsync(string EmployePassportGuidId)
        {
            var empPassp = await _EmployePassportRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == EmployePassportGuidId);
            if (empPassp == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.EmployePassportNotFound,
                    ErrorMessage = ErrorMessage.EmployePassportNotFound
                };
            }
            var address = empPassp.Address;
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOfWork.Addresses.DeleteAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();

                    empPassp.Address = null;
                    _unitOfWork.EmployesPassports.UpdateAsync(empPassp);
                    await _unitOfWork.EmployesPassports.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressDeleteError,
                        ErrorMessage = ErrorMessage.AddressDeleteError
                    };
                }
            }
        }

        public async Task<BaseResult<AddressDto>> RemoveAddressInMarketAsync(string Name)
        {
            var market = await _MarketRepository.GetAll().Include(p=>p.Adress).FirstOrDefaultAsync(p => p.Name == Name);
            if(market == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            var address = market.Adress;
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound 
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOfWork.Addresses.DeleteAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();

                    market.Adress = null;
                    _unitOfWork.Markets.UpdateAsync(market);
                    await _unitOfWork.Markets.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressDeleteError,
                        ErrorMessage = ErrorMessage.AddressDeleteError
                    };
                }
            }
        }
        public async Task<BaseResult<AddressDto>> RemoveAddressInMarketGuidAsync(string MarketGuidId)
        {
            var market = await _MarketRepository.GetAll().Include(P=>P.Adress).FirstOrDefaultAsync(p=>p.GuidId== MarketGuidId);
            if(market == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.MarketNotFound,
                    ErrorMessage = ErrorMessage.MarketNotFound
                };
            }
            if(market.Adress == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == market.Adress.GuidId);
            if(address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOfWork.Addresses.DeleteAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();

                    market.Adress = null;
                    _unitOfWork.Markets.UpdateAsync(market);
                    await _unitOfWork.Markets.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressDeleteError,
                        ErrorMessage = ErrorMessage.AddressDeleteError
                    };
                }
            }
        }

        public async Task<BaseResult<AddressDto>> RemoveAddressInUserAsync(string Login, string Password, string addressGuidId)
        {
            var user = await _UserRepository.GetAll().Include(p=>p.Adresses).FirstOrDefaultAsync(p => p.Login == Login);
            if(user == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p => p.GuidId == addressGuidId);
            if (address == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound
                };
            }
            
            var addressUser = user.Adresses.FirstOrDefault(p=>p.UserId == user.Id);
            if(addressUser == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                    ErrorMessage = ErrorMessage.AddressNotFound,
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOfWork.Addresses.DeleteAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();

                    user.Adresses.Remove(address);
                    _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.Users.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressDeleteError,
                        ErrorMessage = ErrorMessage.AddressDeleteError
                    };
                }
            }
        }
        public async Task<BaseResult<AddressDto>> RemoveAddressInUserGuidAsync(string userGuidId,string Password, string addressGuidId)
        {
            var user = await _UserRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == userGuidId);
            if (user == null)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                };
            }
            
            var address = await _AddressRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == addressGuidId);
            if (address == null && address.UserId != user.Id)
            {
                return new BaseResult<AddressDto>
                {
                    ErrorMessage = ErrorMessage.AddressNotFound,
                    ErrorCode = (int)ErrorCodes.AddressNotFound,
                };
            }
            using(var transaction = await _unitOfWork.BeginTransitionAsync())
            {
                try
                {
                    _unitOfWork.Addresses.DeleteAsync(address);
                    await _unitOfWork.Addresses.SaveChangesAsync();

                    user.Adresses.Remove(address);
                    _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.Users.SaveChangesAsync();

                    await transaction.CommitAsync();

                    _messageProducer
                        .SendMessage(address, nameof(HttpMethods.DELETE), _rabbitOptions.Value.RoutingKey, _rabbitOptions.Value.ExchangeName);
                    return new BaseResult<AddressDto>
                    {
                        Data = _mapper.Map<AddressDto>(address)
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.Error(ex, ex.Message);
                    return new BaseResult<AddressDto>
                    {
                        ErrorCode = (int)ErrorCodes.AddressDeleteError,
                        ErrorMessage = ErrorMessage.AddressDeleteError
                    };
                }
            }
        }
    }
}
