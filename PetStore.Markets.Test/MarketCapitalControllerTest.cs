using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetStore.Markets.API.Controllers;
using PetStore.Markets.Application.Service;
using PetStore.Markets.DAL;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Entity;
using Serilog;
using PetStore.Markets.Application.Mapping;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Result;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Dto.MarketCapital;
using PetStore.Markets.Domain.Dto.Market;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace PetStore.Markets.Test
{
    public class MarketCapitalControllerTest
    {
        private readonly MarketCapitalController _controller;
        public MarketCapitalControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwertyuiop")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var ProductRepository = new BaseRepository<Product>(DbContext);
            var UserRepository = new BaseRepository<User>(DbContext);
            var AddressRepository = new BaseRepository<Address>(DbContext);
            var EmployeRepository = new BaseRepository<Employe>(DbContext);
            var EmployePassportRepository = new BaseRepository<EmployePassport>(DbContext);
            var MarketRepository = new BaseRepository<Market>(DbContext);
            var ProductLineRepository = new BaseRepository<ProductLine>(DbContext);
            var marketCaptailsRepository = new BaseRepository<MarketCapital>(DbContext);
            var marketCaptailsProductLinesRepository = new BaseRepository<MarketCapitalProductLine>(DbContext);

            var unitOfWork = new UnitOfWork(DbContext, UserRepository, MarketRepository, AddressRepository
                , ProductLineRepository, EmployeRepository, EmployePassportRepository, marketCaptailsRepository
                , marketCaptailsProductLinesRepository);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MarketMapping());
                mc.AddProfile(new MarketCapitalMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var markCapitalService = new MarketCapitalService(marketCaptailsRepository, ProductLineRepository, 
                ProductRepository, MarketRepository, mapper, logger.Object,cache, unitOfWork); 
            _controller = new MarketCapitalController(markCapitalService);
        }
        [Fact]
        public async Task GetAll_ProductLinesInMarketCapitalInDaily_IsOk_Test()
        {
            //Arrange
            //Act
            var result = await _controller.GetProductsLinesGuidAsync("o440ca5f-cb42-40f9-b180-c7bd81c15630");
            //Assert
            var actionResult = Assert.IsType<ActionResult<CollectionResult<ProductLineDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<CollectionResult<ProductLineDto>>(okRequestResult.Value);
        }
        [Fact]
        public async Task CRUD_MarketCapital_IsOkTest()
        {
            //Arrange
            var dtoProdLine = new MarketCapitalProductLineDto("Name",DateTime.UtcNow.AddHours(3).ToString(),2, "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            //Act
            var resultProdLine = await _controller.AddProductLineInMarketAsync(dtoProdLine);
            //Assert
            var actionResultProdLine = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLine);
            var okRequestResultProdLine = Assert.IsType<OkObjectResult>(actionResultProdLine.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLine.Value);

            //Arrange
            //Act
            var result = await _controller.GetMarketCapitalAsync(DateTime.UtcNow.AddHours(3).ToString(), "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            //Assert
            var actionResult = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResult.Value);

            //Arrange
            var dtoProdLinePlus = new ProductLineNameDto("o550ca5f-cb42-40f9-b180-c7bd81c15630", DateTime.UtcNow.ToString(), "Name");
            //Act
            var resultProdLinePlus = await _controller.PlusProductLineInMarketAsync(dtoProdLinePlus);
            var actionResultProdLinePlus = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLinePlus);
            var okRequestResultProdLinePlus = Assert.IsType<OkObjectResult>(actionResultProdLinePlus.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLinePlus.Value);

            //Arrange
            //Act
            var resultProdLineMinus = await _controller.MinusProductLineInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", DateTime.UtcNow.ToString(), "Name");
            //Assert
            var actionResultProdLineMinus = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLineMinus);
            var okRequestResultProdLineMinus = Assert.IsType<OkObjectResult>(actionResultProdLineMinus.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLineMinus.Value);

            //Arrange
            //Act
            var resultProdLineRemove = await _controller.RemoveProductLineInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", DateTime.UtcNow.ToString(), "Name");
            //Assert
            var actionResultProdLineRemove = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLineRemove);
            var okRequestResultProdLineRemove = Assert.IsType<OkObjectResult>(actionResultProdLineRemove.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLineRemove.Value);
        }
    }
}
