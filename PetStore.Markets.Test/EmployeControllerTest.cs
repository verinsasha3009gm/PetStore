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
using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Dto.Employe;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using PetStore.Markets.Domain.Settings;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Test
{
    public class EmployeControllerTest
    {
        private readonly EmployeController _controller;
        public EmployeControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwertyuiop")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var UserRepository = new BaseRepository<User>(DbContext);
            var AddressRepository = new BaseRepository<Address>(DbContext);
            var EmployeRepository = new BaseRepository<Employe>(DbContext);
            var EmployePassportRepository = new BaseRepository<EmployePassport>(DbContext);
            var TokenRepository = new BaseRepository<Token>(DbContext);
            var MarketRepository = new BaseRepository<Market>(DbContext);
            var ProductLineRepository = new BaseRepository<ProductLine>(DbContext);
            var marketCaptails = new BaseRepository<MarketCapital>(DbContext);
            var marketCaptailsProductLines = new BaseRepository<MarketCapitalProductLine>(DbContext);

            var unitOfWork = new UnitOfWork(DbContext, UserRepository, MarketRepository, AddressRepository
                , ProductLineRepository, EmployeRepository, EmployePassportRepository, marketCaptails
                , marketCaptailsProductLines);

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
            var tokenService = new TokenService(opt, EmployeRepository); 
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EmployeMapping());
                mc.AddProfile(new EmployePassportMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var empPasspService = new EmployePassportService(EmployePassportRepository,EmployeRepository,mapper
                ,logger.Object, cache, unitOfWork);
            var empService = new EmployeService(EmployeRepository,TokenRepository,tokenService,empPasspService
                ,mapper,logger.Object, cache);
            _controller = new EmployeController(empService);
        }
        [Fact]
        public async Task GetAll_Employe_IsOk_Test()
        {
            //Arrange
            //Act
            var resultGet = await
                _controller.GetAllEmployeAsync();
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<CollectionResult<EmployeDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<CollectionResult<EmployeDto>>(okRequestResultGet.Value);
        }
        [Fact]
        public async Task Logik_Employe_IsOk_Test()
        {
            //Arrange
            var rand = new Random().Next(10000);
            var dtoReg = new RegistrationEmployeDto($"NewEmp#{rand}","M",$"NewEmp{rand}@gmail.com","qwertyuiop","qwertyuiop","Casier",1.0m,1);
            //Act
            var resultReg = await
                _controller.RegistrationEmployeAsync(dtoReg);
            //Assert
            var actionResultReg = Assert
            .IsType<ActionResult<BaseResult<EmployeDto>>>(resultReg);
            var okRequestResultReg = Assert.IsType<OkObjectResult>(actionResultReg.Result);
            Assert.IsType<BaseResult<EmployeDto>>(okRequestResultReg.Value);

            //Arrange
            var dtoLog = new LoginEmployeDto($"NewEmp{rand}@gmail.com", "qwertyuiop");
            //Act
            var resultLog = await _controller.LoginEmployeAsync(dtoLog);
            //Assert
            var actionResultLog = Assert.IsType<ActionResult<BaseResult<TokenDto>>>(resultLog);
            var okRequestResultLog = Assert.IsType<OkObjectResult>(actionResultLog.Result);
            Assert.IsType<BaseResult<TokenDto>>(okRequestResultLog.Value);

            //Arrange
            //Act
            var resultGet = await _controller.GetEmployeAsync($"NewEmp{rand}@gmail.com");
            //Assert
            var actionResultGet = Assert.IsType<ActionResult<BaseResult<EmployeDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<EmployeDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultGetGuid = await _controller.GetEmployeGuidIdAsync($"NewEmp{rand}@gmail.com","qwertyuiop");
            //Assert
            var actionResultGetGuid = Assert.IsType<ActionResult<BaseResult<EmployeGuidDto>>>(resultGetGuid);
            var okRequestResultGetGuid = Assert.IsType<OkObjectResult>(actionResultGetGuid.Result);
            Assert.IsType<BaseResult<EmployeGuidDto>>(okRequestResultGetGuid.Value);

            //Arrange
            var dtoUpdate = new UpdateEmployeDto("Emelia", "M", $"NewEmp{rand}@gmail.com", "qwertyuiop");
            //Act
            var resultUpdate = await _controller.UpdateEmployeAsync(dtoUpdate);
            //Assert
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<EmployeDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<EmployeDto>>(okRequestResultUpdate.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.DeleteEmployeAsync($"NewEmp{rand}@gmail.com","qwertyuiop");
            //Assert
            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<EmployeDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<EmployeDto>>(okRequestResultDelete.Value);
        }
    }
}
