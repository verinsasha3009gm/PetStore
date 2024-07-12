using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Markets.Application.Mapping;
using PetStore.Markets.Application.Service;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.DependencyInjection
{
    public static class DependenyInjection
    {
        public static void AddDependencyInjection(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddAutoMapper(typeof(AddressMapping));
            service.AddAutoMapper(typeof(EmployeMapping));
            service.AddAutoMapper(typeof(EmployePassportMapping));

            service.AddAutoMapper(typeof(MarketCapitalMapping));
            service.AddAutoMapper(typeof(MarketMapping));
            service.AddAutoMapper(typeof(PassportMapping));

            service.AddAutoMapper(typeof(ProductLineMapping));
            service.AddAutoMapper(typeof(ProductMapping));
            service.AddAutoMapper(typeof(UserMapping));

            var options = configuration.GetSection(nameof(RedisSettings));
            var redisUrl = options["Url"];
            var instanceName = options["InstanceName"];

            service.AddStackExchangeRedisCache(options => {
                    options.Configuration = redisUrl;
                    options.InstanceName = instanceName;
            });
            

            service.Initialize();
        }
        private static void Initialize(this IServiceCollection service)
        {
            service.AddScoped<IUserService,UserService>();
            service.AddScoped<IEmployeService,EmployeService>();
            service.AddScoped<IEmployePassportService, EmployePassportService>();

            service.AddScoped<IProductLineService,ProductLineService>();
            service.AddScoped<IProductService,ProductService>();
            service.AddScoped<IPassportService,PassportService>();

            service.AddScoped<IAddressService,AddressService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IMarketCapitalService, MarketCapitalService>();

            service.AddScoped<IMarketService, MarketService>();
            service.AddTransient<ICacheService, CacheService>();

        }
    }
}
