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
using PetStore.Users.Domain.Dto;
using PetStore.Users.Domain.Dto.User;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Settings;
using Serilog;

namespace PetStore.Users.Tests
{
    public class UserTests
    {
        private readonly UserController _controller;
        public UserTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;

            var DbContext = new ApplicationDbContext(options);
            var UserRepository = new BaseRepository<Users.Domain.Entity.User>(DbContext);
            var RoleRepository = new BaseRepository<Role>(DbContext);
            var TokenRepository = new BaseRepository<Token>(DbContext);
            var UserRoleRepository = new BaseRepository<UserRole>(DbContext);

            var jwtSettings = new JwtSettings()
            {
                JwtKey = "VDdYF0TsFr2zAIMuNAzEgIDxaEXuO7bm",
                Audience = "PetStore",
                Authority = "PetStore",
                Issuer = "PetStore",
                RefreshTokenValidityInDays = 7,
                Lifitime = 15
            };
            var opt = Options.Create<JwtSettings>(jwtSettings);
            var tokenService = new TokenService(opt, UserRepository);

            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var userService = 
                new UserService(UserRepository,RoleRepository,UserRoleRepository,TokenRepository,tokenService,mapper,logger.Object,cache);
            _controller = new(userService);
        }
        [Fact]
        public async Task GetUserTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var resultGetAll = await
                _controller.GetAllUsersAsync();
            var actionResultGetAll = Assert
            .IsType<ActionResult<CollectionResult<UserDto>>>(resultGetAll);
            var okRequestResultGetAll = Assert.IsType<OkObjectResult>(actionResultGetAll.Result);
            Assert.IsType<CollectionResult<UserDto>>(okRequestResultGetAll.Value);

            var resultGet = await
                _controller.GetUserAsync("TestLogin");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultGet.Value);

            var resultGetGuid = await
                _controller.GetUserGuidIdAsync("TestLogin","qwertyuiop");
            var actionResultGetGuid = Assert
            .IsType<ActionResult<BaseResult<UserGuidDto>>>(resultGetGuid);
            var okRequestResultGetGuid = Assert.IsType<OkObjectResult>(actionResultGetGuid.Result);
            Assert.IsType<BaseResult<UserGuidDto>>(okRequestResultGetGuid.Value);
        }
        [Fact]
        public async Task LogickUserTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var rand = new Random();
            var dtoReg = new RegistrationUserDto($"Test{rand.Next(12121)}", "qwertyuiop@gmail.com", "qwertyuiop", "qwertyuiop");
            var resultReg = await
                _controller.RegistrationUserAsync(dtoReg);
            var actionResultReg = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultReg);
            var okRequestResultReg = Assert.IsType<OkObjectResult>(actionResultReg.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultReg.Value);

            var dtoLg = new LoginUserDto("qwertyuiop@gmail.com","qwertyuiop");
            var resultLg = await
                _controller.LoginUserAsync(dtoLg);
            var actionResultLg = Assert
            .IsType<ActionResult<BaseResult<TokenDto>>>(resultLg);
            var okRequestResultLg = Assert.IsType<OkObjectResult>(actionResultLg.Result);
            Assert.IsType<BaseResult<TokenDto>>(okRequestResultLg.Value);

            var dtoUpdate = new UpdateUserDto("qwertyuiop@gmail.com", "qwertyuiop", $"{dtoReg.Login}", "qwertyuiortryp@gmail.com", "qwertyuiop2");
            var resultUpdate = await
                _controller.UpdateUserAsync(dtoUpdate);
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultUpdate.Value);

            var resultDelete = await
                _controller.DeleteUserAsync($"{dtoUpdate.NewEmail}", $"{dtoUpdate.NewPassport}");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<UserDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<UserDto>>(okRequestResultDelete.Value);
        }
    }
}
