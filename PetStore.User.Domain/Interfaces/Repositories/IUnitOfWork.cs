using Microsoft.EntityFrameworkCore.Storage;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransitionAsync();
        IBaseRepository<Product> Products { get; set; }
        IBaseRepository<User> Users { get; set; }
        IBaseRepository<UserRole> UserRoles { get; set; }
        IBaseRepository<Role> Roles { get; set; }
        IBaseRepository<Address> Addresses { get; set; }
        IBaseRepository<Cart> Carts { get; set; }
        IBaseRepository<CartLine> CartLines { get; set; }
    }
}
