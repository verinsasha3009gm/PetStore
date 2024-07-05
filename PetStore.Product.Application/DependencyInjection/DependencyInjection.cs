using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(CategoryMapping));
            services.AddAutoMapper(typeof(DescriptionMapping));
            services.AddAutoMapper(typeof(ProductMapping));
            services.AddAutoMapper(typeof(ProductPassportMapping));
            services.AddAutoMapper(typeof(TegMapping));
            var options = configuration.GetSection(nameof(RedisSettings));
            var redisUrl = options["Url"];
            var instanceName = options["InstanceName"];

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = redisUrl;
                options.InstanceName = instanceName;
            });
            services.Initialize();
        }
        public static void Initialize(this IServiceCollection services)
        {
            services.AddScoped<IProductTegService, ProductTegService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITegService, TegService>();
            services.AddScoped<IDescriptionService, DescriptionService>();
            services.AddScoped<IProductPassportService, ProductPassportService>();
            services.AddScoped<ICacheService, CacheService>();
        }
    }
}
