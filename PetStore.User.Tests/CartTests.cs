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
            var repositoryMockCart= new BaseRepository<Cart>(DbContext);
            var repositoryMockUser = new BaseRepository<Users.Domain.Entity.User>(DbContext);
            var repositoryMockCartLine = new BaseRepository<CartLine>(DbContext);
            var repositoryMockProduct = new BaseRepository<Product>(DbContext);
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
            ICartService cartService = new CartService(repositoryMockCart
                , repositoryMockUser, repositoryMockCartLine,repositoryMockProduct, mapper, logger.Object,cache);
            _controller = new(cartService);
        }
        [Fact]
        public async Task GetTests()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var result = await
                _controller.GetUserCartAsync("TestLogin");
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<CartDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<CartDto>>(okRequestResult.Value);

            var resultAll = await
                _controller.GetUserAllCartLinesAsync("TestLogin");

            var actionResultAll = Assert
            .IsType<ActionResult<CollectionResult<CartLineDto>>>(resultAll);
            var okRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<CollectionResult<CartLineDto>>(okRequestResultAll.Value);
        }
        [Fact]
        public async Task AddRemoveCartTests()
        {
            var random = new Random();
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dto = new CartLineUserDto("TestLogin", "00000000-0000-0000-0000-000000000003");
            var result = await
                _controller.AddUserCartLinesAsync(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<CartLineDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<CartLineDto>>(okRequestResult.Value);
            //
            //необходимо дать новое айди удаления
            //
            var resultAll = await
                _controller.RemoveUserCartLineAsync("00000000-0000-0000-0000-000000000003", "TestLogin");

            var actionResultAll = Assert
            .IsType<ActionResult<BaseResult<CartLineDto>>>(resultAll);
            var okRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<BaseResult<CartLineDto>>(okRequestResultAll.Value);
        }
        [Fact]
        public async Task ClearCartTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var result = await
                _controller.ClearUserCartAsync("TestLogin");
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<CartDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<CartDto>>(okRequestResult.Value);
        }
    }
}
