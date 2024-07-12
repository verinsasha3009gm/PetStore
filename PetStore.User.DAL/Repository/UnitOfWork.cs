using Microsoft.EntityFrameworkCore.Storage;
using PetStore.Users.Domain.Entity;
using PetStore.Users.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Product> Products { get; set; }
        public IBaseRepository<User> Users { get; set; }
        public IBaseRepository<UserRole> UserRoles { get; set; }
        public IBaseRepository<Role> Roles { get; set; }
        public IBaseRepository<Address> Addresses { get; set; }
        public IBaseRepository<Cart> Carts { get; set; }
        public IBaseRepository<CartLine> CartLines { get; set; }
        public UnitOfWork(ApplicationDbContext context, IBaseRepository<Product> products, IBaseRepository<User> users,
            IBaseRepository<UserRole> userRoles,IBaseRepository<Role> roles, IBaseRepository<Address> addresses
            , IBaseRepository<Cart> carts, IBaseRepository<CartLine> cartLines)
        {
            _context = context;
            Products = products;
            Users = users;
            UserRoles = userRoles;
            Roles = roles;
            Addresses = addresses;
            CartLines = cartLines;
            Carts = carts;
        }

        public async Task<IDbContextTransaction> BeginTransitionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
