using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PetStore.Users.API.Controllers;
using PetStore.Users.Application.Services;
using PetStore.Users.DAL;
using PetStore.Users.DAL.Repository;
using PetStore.Users.Domain.Dto;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Tests
{
    public class TokenTests
    {
        //private readonly TokenController _controller;
        //public TokenTests()
        //{
        //    var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
        //    IDistributedCache distrCache = new MemoryDistributedCache(opts);
        //    var cache = new RedisCacheService(distrCache);
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
        //        .Options;
        //    var DbContext = new ApplicationDbContext(options);
        //    var UserRepository = new BaseRepository<Users.Domain.Entity.User>(DbContext);
        //    var jwtSettings = new JwtSettings()
        //    {
        //        JwtKey = "VDdYF0TsFr2zAIMuNAzEgIDxaEXuO7bm",
        //        Audience ="PetStore",
        //        Authority ="PetStore",
        //        Issuer = "PetStore",
        //        RefreshTokenValidityInDays = 7,
        //        Lifitime =15
        //    };
        //    var opt = Options.Create<JwtSettings>(jwtSettings);
        //    var tokenService = new TokenService(opt,UserRepository);
        //    _controller = new(tokenService);
        //}
        //[Fact]
        //public async Task TokenTest()
        //{
        //    _controller.ModelState.AddModelError("FirstName", "Required");
        //    var dto = new TokenDto()
        //    {
        //         AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDY" +
        //         "vaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jb" +
        //         "GFpbXMvbmFtZSI6IlRlc3RMb2dpbiIsImV4cCI6MTcxNjI5MzA2NywiaXNzIjoiUGV0U3RvcmUiLCJhdWQiOiJQZXRTdG9yZSJ9.nBOdtoM" +
        //         "B7MsnfU-tzOt3RtbqPcgkASJpuIf3Hg5_Y-g",
        //         RefreshToken = "kXYUB5TkvhOGEJfr3dDcLSV99o/bXYoqXGTSeFyX6Lk="
        //    };
        //    var result = await
        //        _controller.RefreshToken(dto);
        //    var actionResult = Assert
        //    .IsType<ActionResult<BaseResult<TokenDto>>>(result);
        //    var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        //    Assert.IsType<BaseResult<TokenDto>>(okRequestResult.Value);
        //}
    }
}
