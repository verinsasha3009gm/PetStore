using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using PetStore.Users.API.Controllers;
using PetStore.Users.Application.Mapping;
using PetStore.Users.Application.Services;
using PetStore.Users.DAL;
using PetStore.Users.DAL.Repository;
using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Services;
using PetStore.Users.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Tests
{
    public class ProductTests
    {
        private readonly ProductController _controller;
        public ProductTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryMockProduct = new BaseRepository<Product>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            IProductService prodService = new ProductService(repositoryMockProduct, mapper, logger.Object, cache);
            _controller = new(prodService);
        }
        [Fact]
        public async Task CRUD_Product_Test()
        {
            //Arrange
            var dto = new CreateProductDto("TestProduct#2#","TestDescription","",121);
            //Act
            var result = await
                _controller.CreateProductAsync(dto);
            //Assert
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResult.Value);

            //Arrange
            var dtoUpdate = new UpdateProductDto("TestProduct#2#", "TestProduct#3#", "TestDescriptionNew",12121);
            //Act
            var resultUpdate = await
                _controller.UpdateProductAsync(dtoUpdate);
            //Assert
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultUpdate.Value);

            //Arrange
            //Act
            var resultAll = await
                _controller.DeleteProductAsync("TestProduct#3#");
            //Assert
            var actionResultAll = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(resultAll);
            var okRequestResultAll = Assert.IsType<OkObjectResult>(actionResultAll.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultAll.Value);
        }
        [Fact]
        public async Task GetAll_Product_Test()
        {
            //Arrange
            //Act
            var resultGetAll = await _controller.GetAllProductAsync();
            //Assert
            var actionResultGetAll = Assert
            .IsType<ActionResult<CollectionResult<ProductDto>>>(resultGetAll);
            var okRequestResultGetAll = Assert.IsType<OkObjectResult>(actionResultGetAll.Result);
            Assert.IsType<CollectionResult<ProductDto>>(okRequestResultGetAll.Value);
        }
        [Fact]
        public async Task Get_Product_Test()
        {
            //Arrange
            //Act
            var resultGet = await _controller.GetProductGuidAsync("TestName4");

            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<ProductGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            var Data = Assert.IsType<BaseResult<ProductGuidDto>>(okRequestResultGet.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGetGuid = await _controller.GetProductGuidIdAsync(Data.Data.GuidId);
            //Assert
            var actionResultGetGuid = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(resultGetGuid);
            var okRequestResultGetGuid = Assert.IsType<OkObjectResult>(actionResultGetGuid.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultGetGuid.Value);

            //Arrange
            //Act
            var resultGetProd = await _controller.GetProductAsync(Data.Data.ProdName);
            //Assert
            var actionGetProd = Assert.IsType<ActionResult<BaseResult<ProductDto>>>(resultGetProd);
            var okRequestGetProd = Assert.IsType<OkObjectResult>(actionGetProd.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestGetProd.Value);
        }
        [Fact]
        public async Task BaseCRUD_Product_Test()
        {
            //Arrange
            var dto = new CreateProductDto("TestProduct#223#", "TestDescription", "", 1231);
            //Act
            var result = await
                _controller.CreateProductAsync(dto);
            //Assert
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var DataCreate = Assert.IsType<BaseResult<ProductDto>>(okRequestResult.Value);
            Assert.NotNull(DataCreate);

            //Arange
            //Act
            var resultGet = await _controller.GetProductGuidAsync(DataCreate.Data.Name);
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<ProductGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            var Data = Assert.IsType<BaseResult<ProductGuidDto>>(okRequestResultGet.Value);
            Assert.NotNull(Data);

            //Arange
            //Act
            var resultDelete = await _controller.DeleteProductGuidIdAsync(Data.Data.GuidId);
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<ProductDto>>(okRequestResultDelete.Value);
        }
    }
}
