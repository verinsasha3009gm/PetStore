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
using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Test
{
    public class MarketControllerTest
    {
        private readonly MarketController _controller;
        public MarketControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwerpoiu")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var UserRepository = new BaseRepository<User>(DbContext);
            var AddressRepository = new BaseRepository<Address>(DbContext);
            var MarketRepository = new BaseRepository<Market>(DbContext);
            var ProductRepository = new BaseRepository<Product>(DbContext);
            var MarketCapitalRepository = new BaseRepository<MarketCapital>(DbContext);
            var ProdLineRepository = new BaseRepository<ProductLine>(DbContext);
            var EmployeRepository = new BaseRepository<Employe>(DbContext);
            var EmployePassportRepository = new BaseRepository<EmployePassport>(DbContext);
            var marketCaptailsProductLinesRepository = new BaseRepository<MarketCapitalProductLine>(DbContext);

            var unitOfWork = new UnitOfWork(DbContext,UserRepository, MarketRepository, AddressRepository
                , ProdLineRepository, EmployeRepository, EmployePassportRepository, MarketCapitalRepository
                , marketCaptailsProductLinesRepository);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MarketMapping());
                mc.AddProfile(new ProductLineMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var markCapitalService = new MarketCapitalService(MarketCapitalRepository,ProdLineRepository,
                ProductRepository,MarketRepository,mapper,logger.Object, cache, unitOfWork);
            var markService = new MarketService(MarketRepository, ProdLineRepository, ProductRepository,
                MarketCapitalRepository,markCapitalService, mapper, logger.Object,cache, unitOfWork);
            _controller = new MarketController(markService);
        }
        
        [Fact]
        public async Task CRUD_Market_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new CreateMarketDto("MarketNameTest");
            //Act
            var resultCreate = await _controller.CreateMarketAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<MarketDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var result = await _controller.GetMarketAsync(Data.Data.NameMarket);
            //Assert
            var actionResult = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResult.Value);

            //Arrange
            //Act
            var resultGuid = await _controller.GetMarketGuidAsync(Data.Data.guidId);
            //Assert
            var actionResultGuid = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultGuid);
            var okRequestResultGuid = Assert.IsType<OkObjectResult>(actionResultGuid.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResultGuid.Value);

            //Arrange
            var dtoUpdate = new MarketUpdateDto(Data.Data.guidId,"NewMarketNameTest");
            //Act
            var resultUpdate = await _controller.UpdateMarketAsync(dtoUpdate);
            //Assert
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResultUpdate.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.DeleteMarketAsync(Data.Data.guidId);
            //Assert
            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_ProductLinesInMarket_IsOk_Test()
        {
            //Arrange
            var dtoProdLineInMarket = new MarketProductLineDto("Name",100, "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            //Act
            var resultAddProdLineInMarket = await _controller.AddProductInMarket(dtoProdLineInMarket);
            //Assert
            var actionResultAddProdLineInMarket = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultAddProdLineInMarket);
            var okRequestResultAddProdLineInMarket = Assert.IsType<OkObjectResult>(actionResultAddProdLineInMarket.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequestResultAddProdLineInMarket.Value);

            //Arrange
            var dtoProdLineInMarketPlus = new MarketProductLineDto("Name",1, "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            //Act
            var resultProdLineInMarketPlus = await _controller.PlusProductInMarketAsync(dtoProdLineInMarketPlus);
            //Assert
            var actionProdLineInMarketPlus = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdLineInMarketPlus);
            var okRequestResultProdLineInMarketPlus = Assert.IsType<OkObjectResult>(actionProdLineInMarketPlus.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequestResultProdLineInMarketPlus.Value);

            //Arrange
            //Act
            var resultProdInMarket = await _controller.GetProductInMarketAsync("Name","NameMarket");
            //Assert
            var actionResultProdInMarket = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarket);
            var OkRequestResultProdInMarket  = Assert.IsType<OkObjectResult>(actionResultProdInMarket.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarket.Value);

            //Arrange
            //Act
            var resultProdGuidInMarket = await _controller.GetProductGuidInMarketAsync("o660ca5f-cb42-40f9-b180-c7bd81c15630", "NameMarket");
            //Assert
            var actionResultGuidInMarket = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdGuidInMarket);
            var OkRequestResultGuidInMarket = Assert.IsType<OkObjectResult>(actionResultGuidInMarket.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultGuidInMarket.Value);

            //Arrange
            //Act
            var resultProdInMarketGuid = await _controller.GetProductInMarketGuidAsync("Name", "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            //Assert
            var actionResultProdInMarketGuid = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarketGuid);
            var OkRequestResultProdInMarketGuid = Assert.IsType<OkObjectResult>(actionResultProdInMarketGuid.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarketGuid.Value);

            //Arrange
            //Act
            var resultProdGuidInMarketGuid = await 
                _controller.GetProductGuidInMarketGuidAsync("o660ca5f-cb42-40f9-b180-c7bd81c15630", "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            //Assert
            var actionResultProdGuidInMarketGuid = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdGuidInMarketGuid);
            var OkRequestResultProdGuidInMarketGuid = Assert.IsType<OkObjectResult>(actionResultProdGuidInMarketGuid.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdGuidInMarketGuid.Value);

            //Arrange
            //Act
            var resultProdInMarketMinus = await
                _controller.MinusProductInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", "Name");
            //Assert
            var actionResultProdInMarketMinus = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarketMinus);
            var OkRequestResultProdInMarketMinus = Assert.IsType<OkObjectResult>(actionResultProdInMarketMinus.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarketMinus.Value);

            //Arrange
            //Act
            var resultProdInMarketDelete = await
                _controller.RemoveProductInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", "Name");
            //Assert
            var actionResultProdInMarketDelete = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarketDelete);
            var OkRequestResultProdInMarketDelete = Assert.IsType<OkObjectResult>(actionResultProdInMarketDelete.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarketDelete.Value);
        }
    }
}
