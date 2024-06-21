using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using PetStore.Markets.API.Controllers;
using PetStore.Markets.Application.Mapping;
using PetStore.Markets.Application.Service;
using PetStore.Markets.DAL;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Dto.User;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using Serilog;

namespace PetStore.Markets.Test
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;

            var DbContext = new ApplicationDbContext(options);
            var UserRepository = new BaseRepository<User>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var userService =
                new UserService(UserRepository, mapper, logger.Object, cache);
            _controller = new(userService);
        }
        [Fact]
        public async Task Test()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dtoCreate = new CreateUserDto("qwertyuiop","qwertyuiop","TestLogin","UserEmailTest#@gmail.com");
            var resultCreate = await
                _controller.CreateUserAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultCreate.Value);

            var resultGet = await
                _controller.GetUserAsync("UserEmailTest#@gmail.com");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<UserGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            var Data= Assert.IsType<BaseResult<UserGuidDto>>(okRequestResultGet.Value);
            Assert.NotNull(Data);

            var dtoUpdate = new UpdateUserDto(Data.Data.GuidId, "qwertyuiop", "NewTestLogin", "UserEmailTest#@gmail.com");
            var resultUpdate = await
                _controller.UpdateUserAsync(dtoUpdate);
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultUpdate.Value);

            var dtoUpdateRole = new UpdateUserRoleDto("UserRole",Data.Data.GuidId, "qwertyuiop");
            var resultUpdateRole = await
                _controller.UpdateRoleForUserAsync(dtoUpdateRole);
            var actionResultUpdateRole = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultUpdateRole);
            var okRequestResultUpdateRole = Assert.IsType<OkObjectResult>(actionResultUpdateRole.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultUpdateRole.Value);

            var resultDelete = await
                _controller.DeleteUserAsync(Data.Data.GuidId,"qwertyuiop");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultDelete.Value);
        }
    }
}
