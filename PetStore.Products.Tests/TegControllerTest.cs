using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.API.Controllers;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.ProductTeg;
using PetStore.Products.Domain.Dto.Teg;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Tests
{
    public class TegControllerTest
    {
        private readonly TegController _controller;
        public TegControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Product.Tests;User Id=postgres;Password=qwertyuiop")
               //.UseSnakeCaseNamingConvention()
               .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryTeg = new BaseRepository<Teg>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TegMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var tegService = new TegService(repositoryTeg,mapper);
            _controller = new(tegService);
        }
        [Fact]
        public async Task GetAll_Tegs_IsOk_Test()
        {
            //Arrange

            //Act
            var resultAll = await _controller.GetAllTegsAsync();
            //Assert
            var ActionResultAll = Assert.IsType<ActionResult<CollectionResult<TegDto>>>(resultAll);
            var OkRewuredResultAll = Assert.IsType<OkObjectResult>(ActionResultAll.Result);
            Assert.IsType<CollectionResult<TegDto>>(OkRewuredResultAll.Value);
        }
        [Fact]
        public async Task CRUD_Teg_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new TegDto("NewTestTeg");
            //Act
            var resultCreate = await _controller.CreateTegAsync(dtoCreate);
            //Assert
            var ActionResultCreate = Assert.IsType<ActionResult<BaseResult<TegDto>>>(resultCreate);
            var OkRewuredResultCreate = Assert.IsType<OkObjectResult>(ActionResultCreate.Result);
            Assert.IsType<BaseResult<TegDto>>(OkRewuredResultCreate.Value);

            //Arrange

            //Act
            var resultGet = await _controller.GetTegAsync("NewTestTeg");
            //Assert
            var ActionResultGet = Assert.IsType<ActionResult<BaseResult<TegDto>>>(resultGet);
            var OkRewuredResultGet = Assert.IsType<OkObjectResult>(ActionResultGet.Result);
            Assert.IsType<BaseResult<TegDto>>(OkRewuredResultGet.Value);

            //Arrange
            var dtoUpdate = new UpdateTegDto("NewTestTeg","NewTestTeg2");
            //Act
            var resultUpdate = await _controller.UpdateTegAsync(dtoUpdate);
            //Assert
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<TegDto>>>(resultUpdate);
            var OkRewuredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            Assert.IsType<BaseResult<TegDto>>(OkRewuredResultUpdate.Value);

            //Arrange

            //Act
            var resultDelete = await _controller.DeleteTegAsync("NewTestTeg2");
            //Assert
            var ActionResultDelete = Assert.IsType<ActionResult<BaseResult<TegDto>>>(resultDelete);
            var OkRewuredResultDelete = Assert.IsType<OkObjectResult>(ActionResultDelete.Result);
            Assert.IsType<BaseResult<TegDto>>(OkRewuredResultDelete.Value);
        }
    }
}
