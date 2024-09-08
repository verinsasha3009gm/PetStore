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
using PetStore.Users.Domain.Dto.Product;
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
    public class AddressTests
    {
        private readonly AddressController _controller;
        public AddressTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwertyuiop")
                //.UseSnakeCaseNamingConvention()
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
                mc.AddProfile(new AddressMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            IAddressService addressService = new AddressService(repositoryAddress
                , repositoryUser, mapper,logger.Object,cache,UnitOfWork);
            _controller = new(addressService);
        }
        [Fact]
        public async Task AddressTest()
        {
            //Arrange
            //Act
            var resultAll = await _controller.GetAllAddressesInUserAsync("TestLogin");
            //Assert
            var actionResultAll = Assert
            .IsType<ActionResult<CollectionResult<AddressDto>>>(resultAll);
            var badRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<CollectionResult<AddressDto>>(badRequestResultAll.Value);
        }
        [Fact]
        public async Task AddressAddTest()
        {
            //Arrange
            var random = new Random();
            var dto = new AddressInUserDto
                ("TestLogin", $"#TestRegion{random.Next(212321)}", "#TestCountry", "#TestCity", "#TestString");
            //Act
            var result = await
                _controller.AddAddressInUserAsync(dto);
            //Assert
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var Data = Assert.IsType<BaseResult<AddressDto>>(okRequestResult.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetAddressInUserAsync(Data.Data.GuidId, "TestLogin");
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultGet);
            var badRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressDto>>(badRequestResultGet.Value);
            
            //Arrange
            //Act
            var resultDelete = await
                _controller.RemoveAddressInUserAsync(Data.Data.GuidId, "TestLogin");
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
    }
}
