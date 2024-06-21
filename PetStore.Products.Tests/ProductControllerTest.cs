using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Petstore.Products.Producer.Interfaces;
using PetStore.Products.API.Controllers;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using PetStore.Products.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Tests
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        public ProductControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Product.Tests;User Id=postgres;Password=qwerpoiu")
                //.UseSnakeCaseNamingConvention()
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryProduct = new BaseRepository<Product>(DbContext);
            var repositoryCategory = new BaseRepository<Category>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            IMessageProducer producer =new Producer.Producer();
            IOptions<RabbitMqSettings> optRabbit = Options.Create<RabbitMqSettings>(new RabbitMqSettings()
            {
                ExchangeName ="",
                 QueueName ="",
                  RoutingKey = ""
            });
            var cache = new CacheService(distrCache);
            var prodService = new ProductService(repositoryProduct, repositoryCategory, mapper, cache, producer, optRabbit);
            _controller = new ProductController(prodService);
        }
        [Fact]
        public async Task GetAllTest()
        {
            var resultAll = await _controller.GetAllProducts();
            var ActionResultAll = Assert.IsType<ActionResult<CollectionResult<ProductDto>>>(resultAll);
            var OkRewuredResultAll = Assert.IsType<OkObjectResult>(ActionResultAll.Result);
            Assert.IsType<CollectionResult<ProductDto>>(OkRewuredResultAll.Value);

            var resultAllCategory = await _controller.GetProductCategoryAllAsync("name");
            var ActionResultAllCategory = Assert.IsType<ActionResult<CollectionResult<ProductDto>>>(resultAllCategory);
            var OkRewuredResultAllCategory = Assert.IsType<OkObjectResult>(ActionResultAllCategory.Result);
            Assert.IsType<CollectionResult<ProductDto>>(OkRewuredResultAllCategory.Value);
        }
        [Fact]
        public async Task Test()
        {
            var dtoCreate = new CreateProductDto("NewName","drdrdrdr",199,1);
            var resultCreate = await _controller.CreateProductDtoAsync(dtoCreate);
            var ActionResultCreate = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultCreate);
            var OkRequredResultCreate = Assert.IsType<OkObjectResult>(ActionResultCreate.Result);
            Assert.IsType<BaseResult<ProductDto>>(OkRequredResultCreate.Value);

            var resultGet = await _controller.GetProductAsync("NewName");
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<ProductGuidDto>>>(resultGet);
            var OkRequredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            var Data = Assert.IsType<BaseResult<ProductGuidDto>>(OkRequredResultGet.Value);
            Assert.NotNull(Data);

            var dtoUpdate = new UpdateProductDto(Data.Data.GuidId,"NewName2","sfdhh",1999);
            var resultUpdate = await _controller.UpdateProductDtoAsync(dtoUpdate);
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultUpdate);
            var OkRequredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductDto>>(OkRequredResultUpdate.Value);

            var resultDelete = await _controller.DeleteProductDtoAsync("NewName2");
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultDelete);
            var OkRequredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<ProductDto>>(OkRequredResultDelete.Value);

        }
    }
}
