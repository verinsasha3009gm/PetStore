using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Products.Application.Mapping;
using PetStore.Products.Application.Services;
using PetStore.Products.Domain.Interfaces.Services;
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
