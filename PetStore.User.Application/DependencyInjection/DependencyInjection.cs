using Microsoft.Extensions.DependencyInjection;
using PetStore.Users.Application.Mapping;
using PetStore.Users.Application.Services;
using PetStore.Users.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AddressMapping));
            services.AddAutoMapper(typeof(CartLineMapping));
            services.AddAutoMapper(typeof(CartMapping));
            services.AddAutoMapper(typeof(RoleMapping));
            services.AddAutoMapper(typeof(ProductMapping));
            services.AddAutoMapper(typeof(UserMapping));
            services.Initialize();
        }
        public static void Initialize(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICacheService, CacheService>();
        }
    }
}
