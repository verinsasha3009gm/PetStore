using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.API.Controllers;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Dto.ProductTeg;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Tests
{
    public class ProductTegControllerTest
    {
        private readonly ProductTegController _controller;
        public ProductTegControllerTest()
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
                , repositoryProductPassport, repositoryDescription,repositoryCategory);
            var prodTegService = new ProductTegService(repositoryTeg,repositoryProduct,repositoryProductTeg,UnitOfWork);
            _controller = new(prodTegService);
        }
        [Fact]
        public async Task CRUD_ProductTeg_IsOk_Test()
        {
            //Arrange
            var dtoAdd = new ProductTegDto("NameTest","Fruct");
            //Act
            var resultAdd = await _controller.CreateProductTegAsync(dtoAdd);
            //Assert
            var ActionResultAdd = Assert.IsType<ActionResult<BaseResult<ProductTegDto>>>(resultAdd);
            var OkRewuredResultAdd = Assert.IsType<OkObjectResult>(ActionResultAdd.Result);
            Assert.IsType<BaseResult<ProductTegDto>>(OkRewuredResultAdd.Value);

            //Arrange
            var dtoUpdate = new UpdateProductTegDto("NameTest","Fruct","Tomato");
            //Act
            var resultUpdate = await _controller.UpdateProductTegAsync(dtoUpdate);
            //Assert
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductTegDto>>>(resultUpdate);
            var OkRewuredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            var Data = Assert.IsType<BaseResult<ProductTegDto>>(OkRewuredResultUpdate.Value);
            Assert.NotNull(Data);

            //Arrange

            //Act
            var resultRemove = await _controller.RemoveProductTegAsync(Data.Data.prodName,Data.Data.tegName);
            //Assert
            var ActionResultRemove = Assert.IsType<ActionResult<BaseResult<ProductTegDto>>>(resultRemove);
            var OkRewuredResultRemove = Assert.IsType<OkObjectResult>(ActionResultRemove.Result);
            Assert.IsType<BaseResult<ProductTegDto>>(OkRewuredResultRemove.Value);
        }
    }
}
