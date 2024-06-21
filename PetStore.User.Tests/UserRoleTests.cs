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
            var UserRep = new BaseRepository<Domain.Entity.User>(DbContext);
            var RoleRep = new BaseRepository<Role>(DbContext);
            var UserRoleRep = new BaseRepository<UserRole>(DbContext);
            var logger = new Mock<ILogger>();
            var userRoleService = new UserRoleService(UserRep, RoleRep, UserRoleRep, logger.Object);
            _controller = new(userRoleService);
        }
        [Fact]
        public async Task UserRoleTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dtoAdd = new UserRoleDto("TestLogin2", "TestRole");
            var resultAdd = await
                _controller.AddRoleForUserAsync(dtoAdd);
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultAdd);
            var badRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultAdd.Value);

            var dtoUpd = new UpdateUserRoleDto("TestLogin2","TestRole","Admin");
            var resultUpd = await
                _controller.UpdateRoleForUserAsync(dtoUpd);
            var actionResultUpd = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultUpd);
            var badRequestResultUpd = Assert.IsType<OkObjectResult>(actionResultUpd.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultUpd.Value);

            var resultDelete = await
                _controller.DeleteRoleForUserAsync("TestLogin2","Admin");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<UserRoleDto>>>(resultDelete);
            var badRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<UserRoleDto>>(badRequestResultDelete.Value);
        }
    }
}
