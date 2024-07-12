using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Users.DAL.Interceptors;
using PetStore.Users.DAL.Repository;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Repositories;
using PetStore.Users.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.DAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessAssembly(this IServiceCollection services ,IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("UsersDataBase");
            services.AddSingleton<DataInterceptors>();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
            });
            services.Interceptor();
        }
        public static void Interceptor(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBaseRepository<Address>, BaseRepository<Address>>();
            services.AddScoped<IBaseRepository<Cart>, BaseRepository<Cart>>();
            services.AddScoped<IBaseRepository<CartLine>, BaseRepository<CartLine>>();
            services.AddScoped<IBaseRepository<Product>, BaseRepository<Product>>();
            services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
            services.AddScoped<IBaseRepository<Token>, BaseRepository<Token>>();
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseRepository<UserRole>, BaseRepository<UserRole>>();
        }
    }
}
