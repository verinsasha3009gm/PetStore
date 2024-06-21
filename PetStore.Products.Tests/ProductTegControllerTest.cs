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
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Product.Tests;User Id=postgres;Password=qwerpoiu")
                //.UseSnakeCaseNamingConvention()
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryTeg = new BaseRepository<Teg>(DbContext);
            var repositoryProduct = new BaseRepository<Product>(DbContext);
            var repositoryProductTeg = new BaseRepository<ProductTeg>(DbContext);
            var prodTegService = new ProductTegService(repositoryTeg,repositoryProduct,repositoryProductTeg);
            _controller = new(prodTegService);
        }
        [Fact]
        public async Task Test()
        {
            var dtoAdd = new ProductTegDto("NameTest","Fruct");
            var resultAdd = await _controller.CreateProductTegAsync(dtoAdd);
            var ActionResultAdd = Assert.IsType<ActionResult<BaseResult<ProductTegDto>>>(resultAdd);
            var OkRewuredResultAdd = Assert.IsType<OkObjectResult>(ActionResultAdd.Result);
            Assert.IsType<BaseResult<ProductTegDto>>(OkRewuredResultAdd.Value);

            var dtoUpdate = new UpdateProductTegDto("NameTest","Fruct","Tomato");
            var resultUpdate = await _controller.UpdateProductTegAsync(dtoUpdate);
            var ActionResultUpdate = Assert.IsType<ActionResult<BaseResult<ProductTegDto>>>(resultUpdate);
            var OkRewuredResultUpdate = Assert.IsType<OkObjectResult>(ActionResultUpdate.Result);
            var Data = Assert.IsType<BaseResult<ProductTegDto>>(OkRewuredResultUpdate.Value);
            Assert.NotNull(Data);

            var resultRemove = await _controller.RemoveProductTegAsync(Data.Data.prodName,Data.Data.tegName);
            var ActionResultRemove = Assert.IsType<ActionResult<BaseResult<ProductTegDto>>>(resultRemove);
            var OkRewuredResultRemove = Assert.IsType<OkObjectResult>(ActionResultRemove.Result);
            Assert.IsType<BaseResult<ProductTegDto>>(OkRewuredResultRemove.Value);
        }
    }
}
