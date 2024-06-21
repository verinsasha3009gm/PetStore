using Microsoft.Extensions.DependencyInjection;
using Petstore.Products.Producer.Interfaces;
using PetStore.Products.Producer;

namespace PetStore.Products.Producer.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddProducer(this IServiceCollection services)
        {
            services.AddScoped<IMessageProducer, Producer>();
        }
    }
}
