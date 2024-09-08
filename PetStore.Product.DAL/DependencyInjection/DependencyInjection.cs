using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Products.Application.Services;
using PetStore.Products.DAL.Interceptors;
using PetStore.Products.DAL.Repository;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.DAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ProductsDataBase");
            services.AddSingleton<DataInterceptors>();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
            });
            services.Inteseptor();
        }
        public static void Inteseptor(this IServiceCollection services) 
        {
            services.AddScoped<IUnitOFWork, UnitOfWork>();
            services.AddScoped<IBaseRepository<Product>,BaseRepository<Product>>();
            services.AddScoped<IBaseRepository<Description>, BaseRepository<Description>>();
            services.AddScoped<IBaseRepository<ProductPassport>, BaseRepository<ProductPassport>>();
            services.AddScoped<IBaseRepository<Teg>, BaseRepository<Teg>>();
            services.AddScoped<IBaseRepository<Category>,BaseRepository<Category>>();
            services.AddScoped<IBaseRepository<ProductTeg>,BaseRepository<ProductTeg>>();
        }
    }
}
