using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Dto.Cart;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PetStore.Users.Domain.Enum;
using PetStore.Users.Application.Resources;
using Serilog;
using System;

namespace PetStore.Users.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseRepository<Cart> _CartRepository;
        private readonly IBaseRepository<User> _UserRepository;
        private readonly IBaseRepository<CartLine> _CartLineRepository;
        private readonly IBaseRepository<Product> _ProductRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICacheService _CacheService;
        public CartService(IBaseRepository<Cart> CartRepository, IBaseRepository<User> UserRepository,
            IBaseRepository<CartLine> CartLineRepository, IBaseRepository<Product> ProductRepository,
            IUnitOfWork unitOfWork,IMapper mapper,ILogger logger,ICacheService cacheService)
        {
            _CartRepository = CartRepository;
            _UserRepository = UserRepository;
            _CartLineRepository = CartLineRepository;
            _ProductRepository = ProductRepository;
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _CacheService = cacheService;
        }    
        public async Task<BaseResult<CartLineDto>> AddUserCartLineAsync(CartLineUserDto dto)
        {
            var user = await _UserRepository.GetAll().Include(p=>p.Cart).ThenInclude(p=>p.Lines).FirstOrDefaultAsync(p=>p.Login == dto.userLogin);
            if (user == null)
            {
                return new BaseResult<CartLineDto>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage =ErrorMessage.UserNotFound
                };
            }
            var cart = user.Cart;
            if (cart == null)
            {
                user.Cart = new Cart() { UserId = user.Id, Lines = new List<CartLine>() };
                cart = await _CartRepository.CreateAsync(user.Cart);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                _CacheService.Set(user.GuidId, user);
            }
            var cartLine = cart.Lines.FirstOrDefault(p=>p.Product.GuidId == dto.GuidId);
            var prod = _CacheService.Get<Product>(dto.GuidId);
            if (prod == null)
            {
                prod = await _ProductRepository.GetAll().FirstOrDefaultAsync(p=>p.GuidId == dto.GuidId);
                if (prod == null)
                {
                    return new BaseResult<CartLineDto>
                    {
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                        ErrorMessage = ErrorMessage.ProductNotFound
                    };
                }
            }
            if(cartLine == null)
            {
                var newCartLine = new CartLine()
                {
                    Count = 1,
                    Product = prod,
                    Cart = cart
                };
                using(var transaction = await _UnitOfWork.BeginTransitionAsync())
                {
                    try
                    {
                        await _UnitOfWork.CartLines.CreateAsync(newCartLine);
                        _UnitOfWork.Carts.UpdateAsync(cart);
                        await _UnitOfWork.Carts.SaveChangesAsync();
                        _UnitOfWork.Users.UpdateAsync(user);
                        await _UnitOfWork.Users.SaveChangesAsync();
                        await transaction.CommitAsync();
                        _CacheService.Set(user.GuidId, user);
                        return new BaseResult<CartLineDto>
                        {
                            Data = _mapper.Map<CartLineDto>(newCartLine),
                        };
                    }
                    catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.Error(ex,ex.Message);
                        return new BaseResult<CartLineDto>
                        {
                            ErrorCode= (int)ErrorCodes.InternalServerException,
                            ErrorMessage = ErrorMessage.InternalServerException,
                        };
                    }
                }
            }
            try
            {
                cartLine.Count++;
                _CartLineRepository.UpdateAsync(cartLine);
                await _CartLineRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<CartLineDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerException,
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                };
            }
            return new BaseResult<CartLineDto>
            {
                Data = _mapper.Map<CartLineDto>(cartLine),
            };
        }
        public async Task<BaseResult<CartDto>> ClearUserCartAsync(string userLogin)
        {
            var user = await _UserRepository.GetAll().Include(p=>p.Cart).ThenInclude(p=>p.Lines).FirstOrDefaultAsync(p=>p.Login == userLogin);
            if(user == null)
            {
                return new BaseResult<CartDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var Cart = user.Cart;
            if (Cart == null)
            {
                user.Cart = new Cart() { UserId = user.Id, Count =0 };
                await _CartRepository.CreateAsync(user.Cart);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                _CacheService.Set(user.GuidId, user);
            }
            try
            {
                user.Cart.Lines.Clear();
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                _CacheService.Set(user.GuidId, user);
            }
            catch(Exception ex)
            {
                return new BaseResult<CartDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerException,
                    ErrorMessage = ErrorMessage.InternalServerException
                };
            }
            return new BaseResult<CartDto>
            {
                Data = _mapper.Map<CartDto>(user.Cart),
            };
        }
        public async Task<CollectionResult<CartLineDto>> GetUserAllCartLinesAsync(string userLogin)
        {
            var user = await _UserRepository.GetAll()
                .Include(p=>p.Cart)
                .ThenInclude(p=>p.Lines)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p=>p.Login == userLogin);
            if (user == null)
            {
                return new CollectionResult<CartLineDto>
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                };
            }
            var cart = user.Cart;
            if(cart == null)
            {
                return new CollectionResult<CartLineDto>
                {
                    ErrorCode = (int)ErrorCodes.CartNotExist,
                    ErrorMessage = ErrorMessage.CartNotExist
                };
            }
            return new CollectionResult<CartLineDto>
            {
                Data = cart.Lines.Select(p=>new CartLineDto(p.Count.ToString(),p.Product.Name)),
                Count = cart.Count,
            };
        }
        public async Task<BaseResult<CartDto>> GetUserCartAsync(string userLogin)
        {
            var user = await _UserRepository.GetAll().Include(p=>p.Cart).FirstOrDefaultAsync(p=>p.Login == userLogin);
            if(user == null)
            {
                return new BaseResult<CartDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var Cart = user.Cart;
            if(Cart == null)
            {
                Cart = new Cart() { UserId = user.Id,  Count =0, Lines =new List<CartLine>()};
                user.Cart = Cart;
                await _CartRepository.CreateAsync(user.Cart);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
            }
            _CacheService.Refrech<User>(user.GuidId);
            return new BaseResult<CartDto>
            {
                Data = _mapper.Map<CartDto>(Cart),
            };
        }
        public async Task<BaseResult<CartLineDto>> RemoveUserCartLineAsync(string guidId, string userLogin)
        {
            var user = await _UserRepository.GetAll()
                .Include(p => p.Cart)
                .ThenInclude(p => p.Lines)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(p => p.Login == userLogin);
            if(user == null)
            {
                return new BaseResult<CartLineDto>
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound,
                };
            }
            var cartLines = user.Cart.Lines.FirstOrDefault(p => p.Product.GuidId == guidId);
            if(cartLines == null)
            {
                return new BaseResult<CartLineDto>
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFoundInCart,
                    ErrorMessage =ErrorMessage.ProductNotFoundInCart
                };
            }
            try
            {
                user.Cart.Lines.Remove(cartLines);
                _UserRepository.UpdateAsync(user);
                await _UserRepository.SaveChangesAsync();
                //_CacheService.Delete<User>(user.GuidId);
            }
            catch (Exception ex)
            {
                return new BaseResult<CartLineDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerException,
                    ErrorCode = (int)ErrorCodes.InternalServerException
                };
            }
            return new BaseResult<CartLineDto>
            {
                Data = _mapper.Map<CartLineDto>(cartLines)
            };
        }
    }
}
