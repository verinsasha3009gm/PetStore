using Microsoft.EntityFrameworkCore.Storage;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Interfaces.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Interfaces.Repository
{
    public interface IUnitOFWork : IStateSaveChanges
    {
        Task<IDbContextTransaction> BeginTransitionAsync();
        IBaseRepository<Product> Products { get; set; }
        IBaseRepository<Teg> Tegs{ get; set; }
        IBaseRepository<ProductTeg> ProductTegs { get; set; }
        IBaseRepository<Description> Descriptions { get; set; }
        IBaseRepository<Category> Categories { get; set; }
    }
}
