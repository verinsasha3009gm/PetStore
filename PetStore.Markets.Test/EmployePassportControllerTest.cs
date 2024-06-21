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
using PetStore.Markets.Domain.Dto.EmployePassport;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using Serilog;

namespace PetStore.Markets.Test
{
    public class EmployePassportControllerTest
    {
        private readonly EmployePassportController _controller;
        public EmployePassportControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwerpoiu")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var EmpPasspRepository = new BaseRepository<EmployePassport>(DbContext);
            var EmpRepository = new BaseRepository<Employe>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EmployePassportMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var empPassspService = new EmployePassportService(EmpPasspRepository,EmpRepository,mapper,logger.Object, cache);
            _controller = new EmployePassportController(empPassspService);
        }
        [Fact]
        public async Task CreateEmpPassportInEmployeTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dto = new EmployePassportGuidDto("Casier",1,10000, "d222ca5f-cb42-40f9-b180-c7bd81c15651");
            var result = await _controller.CreateEmployePassportInEmployeAsync(dto);

            var actionResult = Assert.IsType<ActionResult<BaseResult<EmployePassportDto>>>(result);
            var okRequestResultReg = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<EmployePassportDto>>(okRequestResultReg.Value);

            var resultGet = await _controller.GetEmployePassportGuidAsync("f333ca5f-cb43-40f9-b180-c7bd81c15651");

            var actionResultGet = Assert.IsType<ActionResult<BaseResult<EmployePassportDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<EmployePassportDto>>(okRequestResultGet.Value);

            var dtoUpdate = new UpdateEmployePassportDto("GoldCasier", 1.1m, 1, "qwertyuiop3@gmail.com", "qwertyuiop");

            var resultUpdate = await _controller.UpdateEmployePassportAsync(dtoUpdate);
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<EmployePassportDto>>> (resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<EmployePassportDto>>(okRequestResultUpdate.Value);

            var resultDelete = await _controller.DeleteEmployePassportAsync("qwertyuiop3@gmail.com", "qwertyuiop");

            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<EmployePassportDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<EmployePassportDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task EmployePassportTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dtoCreate = new CreateEmployePassportDto("Casier", 1,10000, "qwertyuiop4@gmail.com");
            var resultCreate = await _controller.CreateEmployePassportAsync(dtoCreate);
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<EmployePassportDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            Assert.IsType<BaseResult<EmployePassportDto>>(okRequestResultCreate.Value);

            var resultGet = await _controller.GetEmployePassportAsync("qwertyuiop4@gmail.com");
            var actionResultGet = Assert.IsType<ActionResult<BaseResult<EmployePassportGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<EmployePassportGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.DeleteEmployePassportAsync("qwertyuiop4@gmail.com", "qwertyuiop");
            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<EmployePassportDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<EmployePassportDto>>(okRequestResultDelete.Value);
        }
    }
}
