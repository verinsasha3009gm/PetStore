using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using PetStore.Users.API.Controllers;
using PetStore.Users.Application.Mapping;
using PetStore.Users.Application.Services;
using PetStore.Users.DAL;
using PetStore.Users.DAL.Repository;
using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Dto.Cart;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Tests
{
    public class CartTests
    {
        private readonly CartController _controller;
        public CartTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryAddress = new BaseRepository<Address>(DbContext);
            var repositoryUser = new BaseRepository<User>(DbContext);
            var repositoryUserRole = new BaseRepository<UserRole>(DbContext);
            var repositoryProduct = new BaseRepository<Product>(DbContext);
            var repositoryRole = new BaseRepository<Role>(DbContext);
            var repositoryCart = new BaseRepository<Cart>(DbContext);
            var repositoryCartLine = new BaseRepository<CartLine>(DbContext);

            var UnitOfWork = new UnitOfWork(DbContext, repositoryProduct, repositoryUser, repositoryUserRole, repositoryRole, repositoryAddress
                , repositoryCart, repositoryCartLine);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CartMapping());
                mc.AddProfile(new CartLineMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            ICartService cartService = new CartService(repositoryCart
                , repositoryUser, repositoryCartLine,repositoryProduct,UnitOfWork, mapper, logger.Object,cache);
            _controller = new(cartService);
        }
        [Fact]
        public async Task Get_CartInUser_Test()
        {
            //Arrange
            //Act
            var result = await
                _controller.GetUserCartAsync("TestLogin");
            //Assert
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<CartDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<CartDto>>(okRequestResult.Value);
        }
        [Fact]
        public async Task GetAll_CartInUser_Test()
        {
            //Arrange
            //Act
            var resultAll = await
                _controller.GetUserAllCartLinesAsync("TestLogin");
            //Assert
            var actionResultAll = Assert
            .IsType<ActionResult<CollectionResult<CartLineDto>>>(resultAll);
            var okRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<CollectionResult<CartLineDto>>(okRequestResultAll.Value);
        }
        [Fact]
        public async Task AddAndRemove_Cart_Test()
        {
            //Arrange
            var dto = new CartLineUserDto("TestLogin", "00000000-0000-0000-0000-000000000003");
            //Act
            var result = await
                _controller.AddUserCartLinesAsync(dto);
            //Assert
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<CartLineDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<CartLineDto>>(okRequestResult.Value);

            //Arrange
            //Act
            var resultAll = await
                _controller.RemoveUserCartLineAsync("00000000-0000-0000-0000-000000000003", "TestLogin");
            //Assert
            var actionResultAll = Assert
            .IsType<ActionResult<BaseResult<CartLineDto>>>(resultAll);
            var okRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<BaseResult<CartLineDto>>(okRequestResultAll.Value);
        }
        [Fact]
        public async Task Clear_Cart_Test()
        {
            //Arrange
            //Act
            var result = await
                _controller.ClearUserCartAsync("TestLogin");
            //Assert
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<CartDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<CartDto>>(okRequestResult.Value);
        }
    }
}
