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

            var repositoryCategory = new BaseRepository<Category>(DbContext);
            var repositoryProduct = new BaseRepository<Product>(DbContext);
            var repositoryProductTeg = new BaseRepository<ProductTeg>(DbContext);
            var repositoryTeg = new BaseRepository<Teg>(DbContext);
            var repositoryDescription = new BaseRepository<Description>(DbContext);
            var repositoryProductPassport = new BaseRepository<ProductPassport>(DbContext);

            var UnitOfWork = new UnitOfWork(DbContext, repositoryProduct, repositoryTeg, repositoryProductTeg
                , repositoryProductPassport, repositoryDescription,repositoryCategory);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductPassportMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var productPassportService = new ProductPassportService(repositoryProductPassport,repositoryProduct
                ,mapper, cache,UnitOfWork);
            _controller = new ProductPassportController(productPassportService);
        }
        [Fact]
        public async Task CRUD_ProductPassport_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new ProductPassportDto("TestCompany","TestName#111","description");
            //Act
            var resultCreate = await _controller.CreateProductPassportAsync(dtoCreate);
            //Assert
            var ActionResultCreate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultCreate);
            var OkRequredResultCreate = Assert.IsType<OkObjectResult>(ActionResultCreate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultCreate.Value);

            //Arrange

            //Act
            var resultGet = await _controller.GetProductPassportAsync("TestCompany", "TestName#111");
            //Assert
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultGet);
            var OkRequredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultGet.Value);

            //Arrange
            var dtoUpdate = new UpdateProductPassportDto("TestName#111","TestCompany", "TestName#222","descriptionNew","NewTestCompany");
            //Act
            var resultUpdate = await _controller.UpdateProductPassportAsync(dtoUpdate);
            //Assert
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultUpdate);
            var OkRequredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultUpdate.Value);

            //Arrange
            
            //Act
            var resultDelete = await _controller.DeleteProductPassportAsync("TestName#222", "NewTestCompany");
            //Assert
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultDelete);
            var OkRequredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRequredResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_ProductPassportInProduct_IsOk_Test()
        {
            //Arrange
            var dtoCreate =new ProductInProductPassportDto("NameTest", "NameTest", "NovaCompany2");
            //Act
            var resultCreate = await _controller.AddPassportInProductAsync(dtoCreate);
            //Assert
            var ActionResultCreate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultCreate);
            var OkRewuredResultCreate = Assert.IsType<OkObjectResult>(ActionResultCreate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultCreate.Value);

            //Arrange

            //Act
            var resultGet = await _controller.GetPassportInProductAsync("NameTest", "NameTest", "NovaCompany2");
            //Assert
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultGet);
            var OkRewuredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultGet.Value);

            //Arrange
            var dtoUpdate = new ProductInProductPassportDto("NameTest", "NameTest3", "NovaCompany3");
            //Act
            var resultUpdate = await _controller.UpdatePassportInProductAsync(dtoUpdate);
            //Assert
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultUpdate);
            var OkRewuredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultUpdate.Value);

            //Arrange

            //Act
            var resultDelete = await _controller.RemovePassportInProductAsync("NameTest", "NameTest3", "NovaCompany3");
            //Assert
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<ProductPassportDto>>>(resultDelete);
            var OkRewuredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<ProductPassportDto>>(OkRewuredResultDelete.Value);
        }
    }
}
