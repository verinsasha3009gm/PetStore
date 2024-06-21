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
                mc.AddProfile(new MarketCapitalMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var markCapitalService = new MarketCapitalService(MarketCapitalRepository, ProdLineRepository, 
                ProductRepository, MarketRepository, mapper, logger.Object,cache); 
            _controller = new MarketCapitalController(markCapitalService);
        }
        [Fact]
        public async Task GetAllTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var result = await _controller.GetProductsLinesGuidAsync("o440ca5f-cb42-40f9-b180-c7bd81c15630");
            var actionResult = Assert.IsType<ActionResult<CollectionResult<ProductLineDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<CollectionResult<ProductLineDto>>(okRequestResult.Value);
        }
        [Fact]
        public async Task MarketCapitalTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            //изменить эту дтошку чтоб брал и день еще
            var dtoProdLine = new MarketCapitalProductLineDto("Name",DateTime.UtcNow.AddHours(3).ToString(),2, "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            var resultProdLine = await _controller.AddProductLineInMarketAsync(dtoProdLine);
            var actionResultProdLine = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLine);
            var okRequestResultProdLine = Assert.IsType<OkObjectResult>(actionResultProdLine.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLine.Value);

            var result = await _controller.GetMarketCapitalAsync(DateTime.UtcNow.AddHours(3).ToString(), "o550ca5f-cb42-40f9-b180-c7bd81c15630");
            var actionResult = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResult.Value);

            var dtoProdLinePlus = new ProductLineNameDto("o550ca5f-cb42-40f9-b180-c7bd81c15630", DateTime.UtcNow.ToString(), "Name");
            var resultProdLinePlus = await _controller.PlusProductLineInMarketAsync(dtoProdLinePlus);
            var actionResultProdLinePlus = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLinePlus);
            var okRequestResultProdLinePlus = Assert.IsType<OkObjectResult>(actionResultProdLinePlus.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLinePlus.Value);

            var resultProdLineMinus = await _controller.MinusProductLineInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", DateTime.UtcNow.ToString(), "Name");
            var actionResultProdLineMinus = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLineMinus);
            var okRequestResultProdLineMinus = Assert.IsType<OkObjectResult>(actionResultProdLineMinus.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLineMinus.Value);

            var resultProdLineRemove = await _controller.RemoveProductLineInMarketAsync("o550ca5f-cb42-40f9-b180-c7bd81c15630", DateTime.UtcNow.ToString(), "Name");
            var actionResultProdLineRemove = Assert.IsType<ActionResult<BaseResult<MarketCapitalDto>>>(resultProdLineRemove);
            var okRequestResultProdLineRemove = Assert.IsType<OkObjectResult>(actionResultProdLineRemove.Result);
            Assert.IsType<BaseResult<MarketCapitalDto>>(okRequestResultProdLineRemove.Value);
        }
    }
}
