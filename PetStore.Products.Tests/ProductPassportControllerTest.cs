using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PetStore.Products.API.Controllers;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Dto.ProductPassport;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Tests
{
    public class ProductPassportControllerTest
    {
        private readonly ProductPassportController _controller;
        public ProductPassportControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Product.Tests;User Id=postgres;Password=qwerpoiu")
               //.UseSnakeCaseNamingConvention()
               .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryProduct = new BaseRepository<Product>(DbContext);
            var repositoryProductPassport = new BaseRepository<ProductPassport>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductPassportMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var productPassportService = new ProductPassportService(repositoryProductPassport,repositoryProduct,mapper, cache);
            _controller = new ProductPassportController(productPassportService);
        }
        [Fact]
        public async Task Test()
        {
            var dtoCreate =new ProductPassportDto("TestCompany","TestName#111","description");
            var resultCreate = await _controller.CreateProductPassportAsync(dtoCreate);
            var ActionResultCreate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultCreate);
            var OkRequredResultCreate = Assert.IsType<OkObjectResult>(ActionResultCreate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultCreate.Value);

            var resultGet = await _controller.GetProductPassportAsync("TestCompany", "TestName#111");
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultGet);
            var OkRequredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultGet.Value);

            var dtoUpdate = new UpdateProductPassportDto("TestName#111","TestCompany", "TestName#222","descriptionNew","NewTestCompany");
            var resultUpdate = await _controller.UpdateProductPassportAsync(dtoUpdate);
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultUpdate);
            var OkRequredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultUpdate.Value);

            var resultDelete = await _controller.DeleteProductPassportAsync("TestName#222", "NewTestCompany");
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultDelete);
            var OkRequredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultDelete.Value);
        }
        [Fact]
        public async Task ProductPassportInProductTest()
        {
            var dtoCreate =new ProductInProductPassportDto("NameTest", "NameTest", "NovaCompany2");
            var resultCreate = await _controller.AddPassportInProductAsync(dtoCreate);
            var ActionResultCreate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultCreate);
            var OkRewuredResultCreate = Assert.IsType<OkObjectResult>(ActionResultCreate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultCreate.Value);

            var resultGet = await _controller.GetPassportInProductAsync("NameTest", "NameTest", "NovaCompany2");
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultGet);
            var OkRewuredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultGet.Value);

            var dtoUpdate = new ProductInProductPassportDto("NameTest", "NameTest3", "NovaCompany3");
            var resultUpdate = await _controller.UpdatePassportInProductAsync(dtoUpdate);
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultUpdate);
            var OkRewuredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultUpdate.Value);

            var resultDelete = await _controller.RemovePassportInProductAsync("NameTest", "NameTest3", "NovaCompany3");
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultDelete);
            var OkRewuredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultDelete.Value);
        }
    }
}
