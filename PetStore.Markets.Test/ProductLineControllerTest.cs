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
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using Serilog;

namespace PetStore.Markets.Test
{
    public class ProductLineControllerTest
    {
        private readonly ProductLineController _controller;
        public ProductLineControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwertyuiop")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var ProdLineRepository = new BaseRepository<ProductLine>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductLineMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var prodLineService = new ProductLineService(ProdLineRepository,mapper,logger.Object, cache);
            _controller = new ProductLineController(prodLineService);
        }
        [Fact]
        public async Task CRUD_ProductLine_IsOk_Test()
        {
            //Arrange
            var dtoPlus = new ProductLineGuidDto("q222ca5f-cb42-40q9-b180-c7bd81c15651");
            //Act
            var PlusResult = await _controller.PlusProductLineAsync(dtoPlus);
            //Assert
            var actionResultPlus = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(PlusResult);
            var okRequiredResultPlus = Assert.IsType<OkObjectResult>(actionResultPlus.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequiredResultPlus.Value);

            //Arrange
            //Act
            var GetResult = await _controller.GetProductLineAsync("q222ca5f-cb42-40q9-b180-c7bd81c15651");
            //Assert
            var actionResultGet = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(GetResult);
            var okRequiredResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequiredResultGet.Value);

            //Arrange
            //Act
            var MinusResult = await _controller.MinusProductLineAsynAsync("q222ca5f-cb42-40q9-b180-c7bd81c15651");
            //Assert
            var actionResultMinus = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(MinusResult);
            var okRequiredResultMinus = Assert.IsType<OkObjectResult>(actionResultMinus.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequiredResultMinus.Value);
        }
    }
}
