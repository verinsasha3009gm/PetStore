using Microsoft.EntityFrameworkCore.Storage;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransitionAsync();
        IBaseRepository<User> Users { get; set; }
        IBaseRepository<Address> Addresses { get; set; }
        IBaseRepository<Employe> Employes { get; set; }
        IBaseRepository<EmployePassport> EmployesPassports { get; set; }
        IBaseRepository<Market> Markets { get; set; }
        IBaseRepository<MarketCapital> MarketCaptails { get; set; }
        IBaseRepository<ProductLine> ProductLines { get; set; }
        IBaseRepository<MarketCapitalProductLine> MarketCaptailsProductLines { get; set; }
    }
}
