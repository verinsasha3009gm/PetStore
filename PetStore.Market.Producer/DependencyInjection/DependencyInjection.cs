using Microsoft.Extensions.DependencyInjection;
using PetStore.Markets.Producer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Producer.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddProducer(this IServiceCollection services)
        {
            services.AddScoped<IMessageProducer, Producer>();
        }
    }
}
