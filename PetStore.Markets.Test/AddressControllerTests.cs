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
               .UseNpgsql("Server=localhost;Port=5432;Database=PetStore.Market.Tests;User Id=postgres;Password=qwertyuiop")
               .Options;

            var DbContext = new ApplicationDbContext(options);

            var UserRepository = new BaseRepository<User>(DbContext);
            var AddressRepository = new BaseRepository<Address>(DbContext);
            var EmployeRepository = new BaseRepository<Employe>(DbContext);
            var EmployePassportRepository = new BaseRepository<EmployePassport>(DbContext);
            var MarketRepository = new BaseRepository<Market>(DbContext);
            var ProductLineRepository = new BaseRepository<ProductLine>(DbContext);
            var marketCaptails = new BaseRepository<MarketCapital>(DbContext);
            var marketCaptailsProductLines = new BaseRepository<MarketCapitalProductLine>(DbContext);

            var unitOfWork = new UnitOfWork(DbContext, UserRepository, MarketRepository, AddressRepository
                ,ProductLineRepository, EmployeRepository, EmployePassportRepository,marketCaptails
                , marketCaptailsProductLines );

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
                ,MarketRepository, mapper, logger.Object,cache,producer,optRabbit, unitOfWork);
            _controller = new(addressService);
        }
        [Fact]
        public async Task CRUD_Address_Test()
        {
            //Arrange
            var dtoCreate = new AddressDto("qwertyuiop", "qwertyuiop", "qwertyuiop", "qwertyuiop");
            //Act
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetAddressAsync(Data.Data.GuidId);
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultGet.Value);

            //Arrange
            var dtoUpdate = new AddressGuidDto(Data.Data.GuidId,"qwertyuiop2", "qwertyuiop2", "qwertyuiop2", "qwertyuiop2");
            //Act
            var resultUpdate = await
                _controller.UpdateAddressAsync(dtoUpdate);
            //Assert
            var actionResultUpdate = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultUpdate);
            var okRequestResultUpdate = Assert.IsType<OkObjectResult>(actionResultUpdate.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultUpdate.Value);

            //Arrange
            //Act
            var resultDelete = await
                _controller.DeleteAddressAsync(Data.Data.GuidId);
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task GetAll_AddressesInUserGuid_IsOk_Test()
        {
            //Arrange
            //Act
            var resultGetGuidAll = await
                _controller.GetAddressesGuidInUserAsync("e008ca5f-cb42-40f9-b180-c7bd81c15651");
            //Assert
            var actionResultGetGuidAll = Assert
            .IsType<ActionResult<CollectionResult<AddressGuidDto>>>(resultGetGuidAll);
            var okRequestResultGetGuidAll = Assert.IsType<OkObjectResult>(actionResultGetGuidAll.Result);
            Assert.IsType<CollectionResult<AddressGuidDto>>(okRequestResultGetGuidAll.Value);
        }
        [Fact]
        public async Task GetAll_AddressesInUser_IsOk_Test() 
        { 
            //Arrange
            //Act
            var resultGetAll = await
                _controller.GetAddressesInUserAsync("TestLogin", "qwertyuiop");
            //Assert
            var actionResultGetAll = Assert
            .IsType<ActionResult<CollectionResult<AddressDto>>>(resultGetAll);
            var okRequestResultGetAll = Assert.IsType<OkObjectResult>(actionResultGetAll.Result);
            Assert.IsType<CollectionResult<AddressDto>>(okRequestResultGetAll.Value);
        }
        [Fact]
        public async Task CRUD_AddressInUser_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new AddressUserDto("Russia","Moskau","Moskau","Lininskai", "TestLogin", "qwertyuiop");
            //Act
            var resultGetGuidAll = await
                _controller.AddAddressInUserAsync(dtoCreate);
            //Assert
            var actionResultGetGuidAll = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGetGuidAll);
            var okRequestResultGetGuidAll = Assert.IsType<OkObjectResult>(actionResultGetGuidAll.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGetGuidAll.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetUserAddressAsync("TestLogin", Data.Data.GuidId);
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.RemoveAddressInUserAsync("TestLogin", "qwertyuiop",Data.Data.GuidId);
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_UserGuidAddress_IsOk_Test()
        {
            //Arrange
            var rand = new Random();
            var dtoCreate = new AddressDto($"qwertyuiop212#{rand.Next(121212)}", "qwertyuiop1212", "qwertyuiop1221", "qwertyuiop211");
            //Act
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            var dtoAdd = new AddressUserGuidDto("e008ca5f-cb42-40f9-b180-c7bd81c15651", Data.Data.GuidId);
            //Act
            var resultAdd = await
                _controller.AddAddressInUserGuidAsync(dtoAdd);
            //Assert
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultAdd);
            var okRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultAdd.Value);


            //Arrange
            //Act
            var resultGet = await
                _controller.GetAddressGuidInUserAsync("e008ca5f-cb42-40f9-b180-c7bd81c15651", Data.Data.GuidId);
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.RemoveAddressInUserGuidAsync("e008ca5f-cb42-40f9-b180-c7bd81c15651", "qwertyuiop", Data.Data.GuidId);
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_AddressInMarket_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new AddressMarketDto("Russia", "Moskau", "Moskau", "Lininskai", "TestMarket");
            //Act
            var resultCreate = await
                _controller.AddAddressInMarketAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetMarketAddressAsync("TestMarket");
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.RemoveAddressInMarketAsync("TestMarket");
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_AddressInMarketGuid_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new AddressDto("qwertyuiop212", "qwertyuiop1212", "qwertyuiop1221", "qwertyuiop211");
            //Act
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            var dtoAdd = new AddressMarketGuidDto("o440ca5f-cb42-40f9-b180-c7bd81c15630", Data.Data.GuidId);
            //Act
            var resultAdd = await
                _controller.AddAddressInMarketGuidAsync(dtoAdd);
            //Assert
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultAdd);
            var okRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultAdd.Value);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetMarketGuidAddressAsync("o440ca5f-cb42-40f9-b180-c7bd81c15630");
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.RemoveAddressInMarketGuidAsync("o440ca5f-cb42-40f9-b180-c7bd81c15630");
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_AddressInEmployePassport_IsOk_Test()
        {
            //Arrange
            var dtoCreate = new AddressEmployePassportDto("Russia", "Moskau", "Moskau", "Lininskai", "Name");
            //Act
            var resultCreate = await
                _controller.AddAddressInEmployePassportAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetEmployePassportAddressAsync("Name");
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.RemoveAddressInEmployePassportAsync("Name");
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
        [Fact]
        public async Task CRUD_AddressInEmployePassportGuid_IsOk_Test()
        {
            //Arrange
            var rand = new Random();
            var dtoCreate = new AddressDto($"qwertyuiopTest#{rand.Next(121222)}", $"qwertyuiopTest#{rand.Next(121212)}", "qwertyuiopTest", "qwertyuiopTest");
            //Act
            var resultCreate = await
                _controller.CreateAddressAsync(dtoCreate);
            //Assert
            var actionResultCreate = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultCreate);
            var okRequestResultCreate = Assert.IsType<OkObjectResult>(actionResultCreate.Result);
            var Data = Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultCreate.Value);
            Assert.NotNull(Data);

            //Arrange
            var dtoAdd = new AddressEmployePassportGuidDto( Data.Data.GuidId, "f111ca5f-cb42-40f9-b180-c7bd81c15651");
            //Act
            var resultAdd = await
                _controller.AddAddressInEmployePassportGuidAsync(dtoAdd);
            //Assert
            var actionResultAdd = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultAdd);
            var okRequestResultAdd = Assert.IsType<OkObjectResult>(actionResultAdd.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultAdd.Value);

            //Arrange
            //Act
            var resultGet = await
                _controller.GetEmployePassportGuidAddressAsync("f111ca5f-cb42-40f9-b180-c7bd81c15651");
            //Assert
            var actionResultGet = Assert
            .IsType<ActionResult<BaseResult<AddressGuidDto>>>(resultGet);
            var okRequestResultGet = Assert.IsType<OkObjectResult>(actionResultGet.Result);
            Assert.IsType<BaseResult<AddressGuidDto>>(okRequestResultGet.Value);

            //Arrange
            //Act
            var resultDelete = await _controller.RemoveAddressInEmployePassportGuidAsync("f111ca5f-cb42-40f9-b180-c7bd81c15651");
            //Assert
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<AddressDto>>>(resultDelete);
            var okRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<AddressDto>>(okRequestResultDelete.Value);
        }
    }
}
