using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Markets.DAL.Interseptors;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Repositories;

namespace PetStore.Markets.DAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddAccessData(this IServiceCollection service, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MarketData");
            service.AddSingleton<DataInterceptors>();
            service.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
            });
            service.Initialize();
        }
        private static void Initialize(this IServiceCollection service)
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddScoped<IBaseRepository<Product>,BaseRepository<Product>>();
            service.AddScoped<IBaseRepository<User>,BaseRepository<User>>();
            service.AddScoped<IBaseRepository<Employe>,BaseRepository<Employe>>();
            service.AddScoped<IBaseRepository<EmployePassport>,BaseRepository<EmployePassport>>();
            service.AddScoped<IBaseRepository<Passport>,BaseRepository<Passport>>();

            service.AddScoped<IBaseRepository<Address>,BaseRepository<Address>>();
            service.AddScoped<IBaseRepository<Market>,BaseRepository<Market>>();
            service.AddScoped<IBaseRepository<MarketCapital>,BaseRepository<MarketCapital>>();
            service.AddScoped<IBaseRepository<Token>,BaseRepository<Token>>();
            service.AddScoped<IBaseRepository<ProductLine>,BaseRepository<ProductLine>>();
        }
    }
}
