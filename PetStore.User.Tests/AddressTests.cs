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
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
                //.UseSnakeCaseNamingConvention()
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryMockProduct = new BaseRepository<Address>(DbContext);
            var repositoryMockUser = new BaseRepository<Domain.Entity.User>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AddressMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            IAddressService addressService = new AddressService(repositoryMockProduct
                , repositoryMockUser, mapper,logger.Object,cache);
            _controller = new(addressService);
        }
        [Fact]
        public async Task AddressTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var resultAll = await _controller.GetAllAddressesInUserAsync("TestLogin");
            
            var actionResultAll = Assert
            .IsType<ActionResult<CollectionResult<AddressDto>>>(resultAll);
            var badRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<CollectionResult<AddressDto>>(badRequestResultAll.Value);
        }
        [Fact]
        public async Task AddressAddTest()
        {
            var random = new Random();
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dto = new AddressInUserDto
                ("TestLogin", $"#TestRegion{random.Next(212321)}", "#TestCountry", "#TestCity", "#TestString");
            var result = await
                _controller.AddAddressInUserAsync(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var Data = Assert.IsType<BaseResult<AddressDto>>(okRequestResult.Value);
            Assert.NotNull(Data);

            var resultGet = await
                _controller.GetAddressInUserAsync(Data.Data.GuidId, "TestLogin");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultGet);
            var badRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressDto>>(badRequestResultGet.Value);
        
            var resultDelete = await
                _controller.RemoveAddressInUserAsync(Data.Data.GuidId, "TestLogin");

            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
    }
}
