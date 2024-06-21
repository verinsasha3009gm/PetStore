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
            var MarketRepository = new BaseRepository<Market>(DbContext);
            var ProductRepository = new BaseRepository<Product>(DbContext);
            var MarketCapitalRepository = new BaseRepository<MarketCapital>(DbContext);
            var ProdLineRepository = new BaseRepository<ProductLine>(DbContext);
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
                ProductRepository,MarketRepository,mapper,logger.Object, cache);
            var markService = new MarketService(MarketRepository, ProdLineRepository, ProductRepository,
                MarketCapitalRepository,markCapitalService, mapper, logger.Object,cache);
            _controller = new MarketController(markService);
        }
        [Fact]
        public async Task GetMarketTest()
        {
            var dtoCreate = new CreateMarketDto("MarketNameTest");
            var resultCreate = await _controller.CreateMarketAsync(dtoCreate);
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<MarketDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            var result = await _controller.GetMarketAsync(Data.Data.NameMarket);
            var actionResult = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResult.Value);

            var resultGuid = await _controller.GetMarketGuidAsync(Data.Data.guidId);
            var actionResultGuid = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultGuid);
            var okRequestResultGuid = Assert.IsType<OkObjectResult>(actionResultGuid.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResultGuid.Value);

            var dtoUpdate = new MarketUpdateDto(Data.Data.guidId,"NewMarketNameTest");
            var resultUpdate = await _controller.UpdateMarketAsync(dtoUpdate);
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResultUpdate.Value);

            var resultDelete = await _controller.DeleteMarketAsync(Data.Data.guidId);
            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<MarketDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<MarketDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task ProductLinesInMarketTest()
        {
            var dtoProdLineInMarket = new MarketProductLineDto("Name",100, "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            var resultAddProdLineInMarket = await _controller.AddProductInMarket(dtoProdLineInMarket);
            var actionResultAddProdLineInMarket = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultAddProdLineInMarket);
            var okRequestResultAddProdLineInMarket = Assert.IsType<OkObjectResult>(actionResultAddProdLineInMarket.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequestResultAddProdLineInMarket.Value);

            var dtoProdLineInMarketPlus = new MarketProductLineDto("Name",1, "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            var resultProdLineInMarketPlus = await _controller.PlusProductInMarketAsync(dtoProdLineInMarketPlus);
            var actionProdLineInMarketPlus = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdLineInMarketPlus);
            var okRequestResultProdLineInMarketPlus = Assert.IsType<OkObjectResult>(actionProdLineInMarketPlus.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(okRequestResultProdLineInMarketPlus.Value);

            var resultProdInMarket = await _controller.GetProductInMarketAsync("Name","NameMarket");
            var actionResultProdInMarket = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarket);
            var OkRequestResultProdInMarket  = Assert.IsType<OkObjectResult>(actionResultProdInMarket.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarket.Value);

            var resultProdGuidInMarket = await _controller.GetProductGuidInMarketAsync("o660ca5f-cb42-40f9-b180-c7bd81c15630", "NameMarket");
            var actionResultGuidInMarket = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdGuidInMarket);
            var OkRequestResultGuidInMarket = Assert.IsType<OkObjectResult>(actionResultGuidInMarket.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultGuidInMarket.Value);

            var resultProdInMarketGuid = await _controller.GetProductInMarketGuidAsync("Name", "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            var actionResultProdInMarketGuid = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarketGuid);
            var OkRequestResultProdInMarketGuid = Assert.IsType<OkObjectResult>(actionResultProdInMarketGuid.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarketGuid.Value);

            var resultProdGuidInMarketGuid = await 
                _controller.GetProductGuidInMarketGuidAsync("o660ca5f-cb42-40f9-b180-c7bd81c15630", "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            var actionResultProdGuidInMarketGuid = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdGuidInMarketGuid);
            var OkRequestResultProdGuidInMarketGuid = Assert.IsType<OkObjectResult>(actionResultProdGuidInMarketGuid.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdGuidInMarketGuid.Value);

            var resultProdInMarketMinus = await
                _controller.MinusProductInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", "Name");
            var actionResultProdInMarketMinus = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarketMinus);
            var OkRequestResultProdInMarketMinus = Assert.IsType<OkObjectResult>(actionResultProdInMarketMinus.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarketMinus.Value);

            var resultProdInMarketDelete = await
                _controller.RemoveProductInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", "Name");
            var actionResultProdInMarketDelete = Assert.IsType<ActionResult<BaseResult<ProductLineDto>>>(resultProdInMarketDelete);
            var OkRequestResultProdInMarketDelete = Assert.IsType<OkObjectResult>(actionResultProdInMarketDelete.Result);
            Assert.IsType<BaseResult<ProductLineDto>>(OkRequestResultProdInMarketDelete.Value);
        }
    }
}
