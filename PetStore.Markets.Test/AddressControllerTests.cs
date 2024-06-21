using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using PetStore.Markets.API.Controllers;
using PetStore.Markets.Application.Mapping;
using PetStore.Markets.Application.Service;
using PetStore.Markets.DAL;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Dto.Address;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Result;
using PetStore.Markets.Domain.Settings;
using PetStore.Markets.Producer.Interfaces;
using Serilog;

namespace PetStore.Markets.Test
{
    public class AddressControllerTests
    {
        private readonly AddressController _controller;
        public AddressControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwerpoiu")
               .Options;

            var DbContext = new ApplicationDbContext(options);
            var UserRepository = new BaseRepository<User>(DbContext);
            var AddressRepository = new BaseRepository<Address>(DbContext);
            var EmployeRepository = new BaseRepository<Employe>(DbContext);
            var EmployePassportRepository = new BaseRepository<EmployePassport>(DbContext);
            var MarketRepository = new BaseRepository<Market>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AddressMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var logger = new Mock<ILogger>();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new CacheService(distrCache);
            IMessageProducer producer = new Producer.Producer();
            IOptions<RabbitMqSettings> optRabbit = Options.Create<RabbitMqSettings>(new RabbitMqSettings()
            {
                ExchangeName = "",
                QueueName = "",
                RoutingKey = ""
            });
            var addressService =
                new AddressService(AddressRepository,UserRepository,EmployeRepository,EmployePassportRepository
                ,MarketRepository, mapper, logger.Object,cache,producer,optRabbit);
            _controller = new(addressService);
        }
        [Fact]
        public async Task TestAddress()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dtoCreate = new AddressDto("qwertyuiop", "qwertyuiop", "qwertyuiop", "qwertyuiop");
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            var resultGet = await
                _controller.GetAddressAsync(Data.Data.GuidId);
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultGet.Value);

            var dtoUpdate = new AddressGuidDto(Data.Data.GuidId,"qwertyuiop2", "qwertyuiop2", "qwertyuiop2", "qwertyuiop2");
            var resultUpdate = await
                _controller.UpdateAddressAsync(dtoUpdate);
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultUpdate.Value);

            var resultDelete = await
                _controller.DeleteAddressAsync(Data.Data.GuidId);
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task GetTestAlldata()
        {

            _controller.ModelState.AddModelError("FirstName", "Required");

            var resultGetGuidAll = await
                _controller.GetAddressesGuidInUserAsync("e008ca5f-cb42-40f9-b180-c7bd81c15651");
            var actionResultGetGuidAll = Assert
            .IsType<ActionResult<CollectionResult<AddressGuidDto>>>(resultGetGuidAll);
            var okRequestResultGetGuidAll = Assert.IsType<OkObjectResult>(actionResultGetGuidAll.Result);
            Assert.IsType<CollectionResult<AddressGuidDto>>(okRequestResultGetGuidAll.Value);

            var resultGetAll = await
                _controller.GetAddressesInUserAsync("TestLogin", "qwertyuiop");
            var actionResultGetAll = Assert
            .IsType<ActionResult<CollectionResult<AddressDto>>>(resultGetAll);
            var okRequestResultGetAll = Assert.IsType<OkObjectResult>(actionResultGetAll.Result);
            Assert.IsType<CollectionResult<AddressDto>>(okRequestResultGetAll.Value);
        }
        [Fact]
        public async Task TestUserAddress()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var dtoCreate = new AddressUserDto("Russia","Moskau","Moskau","Lininskai", "TestLogin", "qwertyuiop");
            var resultGetGuidAll = await
                _controller.AddAddressInUserAsync(dtoCreate);
            var actionResultGetGuidAll = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGetGuidAll);
            var okRequestResultGetGuidAll = Assert.IsType<OkObjectResult>(actionResultGetGuidAll.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGetGuidAll.Value);
            Assert.NotNull(Data);

            var resultGet = await
                _controller.GetUserAddressAsync("TestLogin", Data.Data.GuidId);
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.RemoveAddressInUserAsync("TestLogin", "qwertyuiop",Data.Data.GuidId);
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task TestUserGuidAddress()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var rand = new Random();
            var dtoCreate = new AddressDto($"qwertyuiop212#{rand.Next(121212)}", "qwertyuiop1212", "qwertyuiop1221", "qwertyuiop211");
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            var dtoAdd = new AddressUserGuidDto("e008ca5f-cb42-40f9-b180-c7bd81c15651", Data.Data.GuidId);
            var resultAdd = await
                _controller.AddAddressInUserGuidAsync(dtoAdd);
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);

            var resultGet = await
                _controller.GetAddressGuidInUserAsync("e008ca5f-cb42-40f9-b180-c7bd81c15651", Data.Data.GuidId);
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.RemoveAddressInUserGuidAsync("e008ca5f-cb42-40f9-b180-c7bd81c15651", "qwertyuiop", Data.Data.GuidId);
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task TestMarketAddress()
        {
            var dtoCreate = new AddressMarketDto("Russia", "Moskau", "Moskau", "Lininskai", "TestMarket");
            var resultCreate = await
                _controller.AddAddressInMarketAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            var resultGet = await
                _controller.GetMarketAddressAsync("TestMarket");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.RemoveAddressInMarketAsync("TestMarket");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task TestMarketGuidAddress()
        {
            var dtoCreate = new AddressDto("qwertyuiop212", "qwertyuiop1212", "qwertyuiop1221", "qwertyuiop211");
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);


            var dtoAdd = new AddressMarketGuidDto("o440ca5f-cb42-40f9-b180-c7bd81c15630", Data.Data.GuidId);
            var resultAdd = await
                _controller.AddAddressInMarketGuidAsync(dtoAdd);
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultAdd);
            var okRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultAdd.Value);

            var resultGet = await
                _controller.GetMarketGuidAddressAsync("o440ca5f-cb42-40f9-b180-c7bd81c15630");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.RemoveAddressInMarketGuidAsync("o440ca5f-cb42-40f9-b180-c7bd81c15630");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task TestEmployePassportAddress()
        {
            var dtoCreate = new AddressEmployePassportDto("Russia", "Moskau", "Moskau", "Lininskai", "Name");
            var resultCreate = await
                _controller.AddAddressInEmployePassportAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            var resultGet = await
                _controller.GetEmployePassportAddressAsync("Name");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.RemoveAddressInEmployePassportAsync("Name");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task TestEmployePassportGuidAddress()
        {
            var rand = new Random();
            var dtoCreate = new AddressDto($"qwertyuiopTest#{rand.Next(121222)}", $"qwertyuiopTest#{rand.Next(121212)}", "qwertyuiopTest", "qwertyuiopTest");
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            var dtoAdd = new AddressEmployePassportGuidDto( Data.Data.GuidId, "f111ca5f-cb42-40f9-b180-c7bd81c15651");
            var resultAdd = await
                _controller.AddAddressInEmployePassportGuidAsync(dtoAdd);
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultAdd);
            var okRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultAdd.Value);

            var resultGet = await
                _controller.GetEmployePassportGuidAddressAsync("f111ca5f-cb42-40f9-b180-c7bd81c15651");
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            var resultDelete = await _controller.RemoveAddressInEmployePassportGuidAsync("f111ca5f-cb42-40f9-b180-c7bd81c15651");
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
    }
}
