using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetStore.Markets.API.Controllers;
using PetStore.Markets.Application.Mapping;
using PetStore.Markets.Application.Service;
using PetStore.Markets.DAL;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Dto.Passport;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using Serilog;

namespace PetStore.Markets.Test
{
    public class PassportControllerTest
    {
        private readonly PassportController _controller;
        public PassportControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwertyuiop")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var PassportRepository = new BaseRepository<Passport>(DbContext);
            var EmployeRepository = new BaseRepository<Employe>(DbContext);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PassportMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();

            var passportService = new PassportService(PassportRepository,EmployeRepository,mapper,logger.Object);
            _controller = new PassportController(passportService);
        }
        [Fact]
        public async Task Get_PassportNames_IsOk_Test()
        {
            //Arrange
            //Act
            var result = await _controller.GetPassportNameAsync("NamePassport", "Familien");
            //Assert
            var actionResult = Assert.IsType<ActionResult<CollectionResult<PassportDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<CollectionResult<PassportDto>>(okRequestResult.Value);
        }
        [Fact]
        public async Task CRUD_Passport_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new CreatePassportDto("Moskay",DateTime.UtcNow.AddDays(-10).ToString(),"Name","Familien");
            //Act
            var resultCreate = await _controller.CreatePassportAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<PassportDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<PassportDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await _controller.GetPassportAsync(Data.Data.PassportSeria, Data.Data.PassportNumber);
            //Assert
            var actionResultGet = Assert.IsType<ActionResult<BaseResult<PassportDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<PassportDto>>(okRequestResultGet.Value);

            //Arrange
            var dtoUpdate = new UpdatePassportDto("Piter", "MVD. SPT.", "werew", "werwerwqe", Data.Data.PassportNumber, Data.Data.PassportSeria);
            //Act
            var resultUpdate = await _controller.UpdatePassportAsync(dtoUpdate);
            //Assert
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<PassportDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Data = Assert.IsType<BaseResult<PassportDto>>(okRequestResultUpdate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultDelete = await _controller.DeletePassportAsync(Data.Data.PassportSeria, Data.Data.PassportNumber);
            //Assert
            var actionResultDelete= Assert.IsType<ActionResult<BaseResult<PassportDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<PassportDto>>(okRequestResultDelete.Value);
        }
    }
}
