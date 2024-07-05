using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.API.Controllers;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.Description;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Tests
{
    public class DescriptionControllerTest
    {
        private readonly DescriptionController _controller;
        public DescriptionControllerTest()
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
                , repositoryProductPassport, repositoryDescription, repositoryCategory);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DescriptionMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var descriptionService = new DescriptionService(repositoryDescription, repositoryProduct, mapper,UnitOfWork);
            _controller = new DescriptionController(descriptionService);
        }
        [Fact]
        public async Task GetAll_Descriptions_IsOk_Test()
        {
            //Arange

            //Act
            var resultGets = await _controller.GetDescriptionsAsync("Name");
            //Assert
            var ActionResultGets = Assert.IsType<ActionResult<CollectionResult<DescriptionDto>>>(resultGets);
            var OkRequredResult = Assert.IsType<OkObjectResult>(ActionResultGets.Result);
            Assert.IsType<CollectionResult<DescriptionDto>>(OkRequredResult.Value);
        }
        [Fact]
        public async Task CRUD_Description_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new DescriptionCultureDto("Name","Ru","cgcuktctu",new List<string> { });
            //Act
            var resultCreate = await _controller.CreateDescriptionAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert.IsType<ActionResult<BaseResult<DescriptionDto>>>(resultCreate);
            var OkRequredResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            Assert.IsType<BaseResult<DescriptionDto>>(OkRequredResultCreate.Value);

            //Arrange

            //Act
            var resultGet = await _controller.GetDescriptionAsync("Name", "Ru");
            //Assert
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<DescriptionDto>>>(resultGet);
            var OkRequredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            Assert.IsType<BaseResult<DescriptionDto>>(OkRequredResultGet.Value);

            //Arrange
            var dtoUpdate = new DescriptionCultureDto("Name","Ru","hugoiug",new List<string> { });
            //Act
            var resultUpdate = await _controller.UpdateDescriptionAsync(dtoUpdate);
            //Assert
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<DescriptionDto>>>(resultUpdate);
            var OkRequredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<DescriptionDto>>(OkRequredResultUpdate.Value);

            //Arrange

            //Act
            var resultDelete = await _controller.DeleteDescriptionsAsync("Name", "Ru");
            //Assert
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<DescriptionDto>>>(resultDelete);
            var OkRequredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<DescriptionDto>>(OkRequredResultDelete.Value);
        }
    }
}
