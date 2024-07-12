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
using PetStore.Markets.Domain.Dto.Product;
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
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        public ProductControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwerpoiu")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var ProductRepository = new BaseRepository<Product>(DbContext);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            var prodService = new ProductService(ProductRepository,mapper,logger.Object, cache);
            _controller = new ProductController(prodService);
        }
        [Fact]
        public async Task CRUD_Product_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new ProductDto("NameTestnew#","Description",12212);
            //Act
            var resultCreate = await _controller.CreateProductAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<ProductGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<ProductGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await _controller.GetProductAsync(Data.Data.GuidId);
            //Assert
            var actionResultGet = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultGet.Value);

            //Arrange
            var dtoUpdate = new ProductGuidDto(Data.Data.GuidId, "NameTestNew#2","DescriptionNew",12212);
            //Act
            var resultUpdate = await _controller.UpdateProductAsync(dtoUpdate);
            //Assert
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultUpdate.Value);
            
            //Arrange
            //Act
            var resultDelete = await _controller.DeleteProductAsync(Data.Data.GuidId);
            //Assert
            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultDelete.Value);
        }
    }
}
