using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.API.Controllers;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.Category;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Tests
{
    public class CategoryControllerTest 
    {
        private readonly CategoryController _controller;
        public CategoryControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Product.Tests;User Id=postgres;Password=qwertyuiop")
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
                ,repositoryProductPassport, repositoryDescription, repositoryCategory);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CategoryMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            var CategoryService = new CategoryService(repositoryCategory, repositoryProduct,mapper,UnitOfWork);
            _controller = new CategoryController(CategoryService);
        }
        [Fact]
        public async Task CRUD_Category_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new CreateCategoryDto("CategoryTestNew12","NewDescription");

            //Act
            var resultCreate = await _controller.CreateCategoryAsync(dtoCreate);

            //Assert
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<CategoryDto>>>(resultCreate);
            var okRequredResuktCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            Assert.IsType<BaseResult<CategoryDto>>(okRequredResuktCreate.Value);

            //Arrange

            //Act
            var resultGet = await _controller.GetCategoryAsync("CategoryTestNew12");

            //Assert
            var actionResultGet = Assert.IsType<ActionResult<BaseResult<CategoryDto>>>(resultGet);
            var okRequredResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<CategoryDto>>(okRequredResultGet.Value);

            //Arrange
            var dtoUpdate = new UpdateCategoryDto("CategoryTestNew12","CategoryTestNew13", "NewDescription");

            //Act
            var resultUpdate = await _controller.UpdateCategoryAsync(dtoUpdate);

            //Assert
            var actionResultUpdate = Assert.IsType<ActionResult<BaseResult<CategoryDto>>>(resultUpdate);
            var okRequredResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<CategoryDto>>(okRequredResultUpdate.Value);

            //Arrange

            //Act
            var resultDelete = await _controller.DeleteCategoryAsync("CategoryTestNew13");

            //Assert
            var actionResultDelete = Assert.IsType<ActionResult<BaseResult<CategoryDto>>>(resultDelete);
            var okRequredResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<CategoryDto>>(okRequredResultDelete.Value);
        }
        [Fact]
        public async Task GetAll_Categories_IsOk_Test()
        {
            //Arrange

            //Act
            var reslt = await _controller.GetAllCategoriesAsync();

            //Assert
            var actionResult = Assert.IsType<ActionResult<CollectionResult<CategoryDto>>>(reslt);
            var okRequredResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<CollectionResult<CategoryDto>> (okRequredResult.Value);
        }
        [Fact]
        public async Task AddAndRemove_ProductInCategory_IsOk_Test()
        {
            //Arrange
            var dtoAdd = new ProductInCategoryDto("name","Name");
            
            //Act
            var resultAdd = await _controller.AddProductInCategory(dtoAdd);

            //Assert
            var actionResultAdd = Assert.IsType<ActionResult<BaseResult<CategoryDto>>>(resultAdd);
            var okRequredResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<CategoryDto>>(okRequredResultAdd.Value);

            //Arrange

            //Act
            var resultRemove = await _controller.RemoveProductInCategoryAsync("name", "Name");

            //Assert
            var actionResultRemove = Assert.IsType<ActionResult<BaseResult<CategoryDto>>>(resultRemove);
            var okRequredResultRemove = Assert.IsType<OkObjectResult>(actionResultRemove.Result);
            Assert.IsType<BaseResult<CategoryDto>>(okRequredResultRemove.Value);
        }
    }
}
