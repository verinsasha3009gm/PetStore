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
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Dto.Role;
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
    public class RoleTests
    {
        private readonly RoleController _controller;
        public RoleTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.User.Tests;User Id=postgres;Password=qwerpoiu")
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryMockRole = new BaseRepository<Role>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RoleMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            IRoleService roleService = new RoleService(repositoryMockRole, mapper, logger.Object);
            _controller = new(roleService);
        }
        [Fact]
        public async Task RoleTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dtoCreate = new RoleDto("NameTestRole");
            var resultCreate = await _controller.CreateRoleAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            Assert.IsType<BaseResult<RoleDto>>(okRequestResultCreate.Value);

            var resultGet = await _controller.GetRoleAsync("NameTestRole");
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(resultGet);
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<RoleDto>>(okRequestResult.Value);

            var dtoUpdate = new UpdateRoleDto("NameTestRole", "NameTestRole#New");
            var resultUpdate = await _controller.UpdateRoleAsync(dtoUpdate);
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<RoleDto>>(okRequestResultUpdate.Value);

            var resultDelete = await _controller.DeleteRoleAsync("NameTestRole#New");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<RoleDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<RoleDto>>(okRequestResultDelete.Value);
        }
    }
}
