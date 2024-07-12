using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using PetStore.Users.API.Controllers;
using PetStore.Users.Application.Services;
using PetStore.Users.DAL;
using PetStore.Users.DAL.Repository;
using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Tests
{
    public class UserRoleTests
    {
        private readonly UserRoleController _controller;
        public UserRoleTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;

            var DbContext = new ApplicationDbContext(options);
            var repositoryAddress = new BaseRepository<Address>(DbContext);
            var repositoryUser = new BaseRepository<User>(DbContext);
            var repositoryUserRole = new BaseRepository<UserRole>(DbContext);
            var repositoryProduct = new BaseRepository<Product>(DbContext);
            var repositoryRole = new BaseRepository<Role>(DbContext);
            var repositoryCart = new BaseRepository<Cart>(DbContext);
            var repositoryCartLine = new BaseRepository<CartLine>(DbContext);

            var UnitOfWork = new UnitOfWork(DbContext, repositoryProduct, repositoryUser, repositoryUserRole, repositoryRole, repositoryAddress
                , repositoryCart, repositoryCartLine);
            var logger = new Mock<ILogger>();
            var userRoleService = new UserRoleService(repositoryUser, repositoryRole, repositoryUserRole, logger.Object,UnitOfWork);
            _controller = new(userRoleService);
        }
        [Fact]
        public async Task CRUD_UserRole_IsOk_Test()
        {
            //Arrange
            var dtoAdd = new UserRoleDto("TestLogin2", "TestRole");
            //Act
            var resultAdd = await
                _controller.AddRoleForUserAsync(dtoAdd);
            //Assert
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultAdd);
            var badRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultAdd.Value);

            //Arrange
            var dtoUpd = new UpdateUserRoleDto("TestLogin2","TestRole","Admin");
            //Act
            var resultUpd = await
                _controller.UpdateRoleForUserAsync(dtoUpd);
            //Assert
            var actionResultUpd = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultUpd);
            var badRequestResultUpd = Assert.IsType<OkObjectResult>(actionResultUpd.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultUpd.Value);

            //Arrange
            //Act
            var resultDelete = await
                _controller.DeleteRoleForUserAsync("TestLogin2","Admin");
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultDelete);
            var badRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultDelete.Value);
        }
    }
}
