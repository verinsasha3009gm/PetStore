using Microsoft.EntityFrameworkCore.Storage;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<User> Users { get; set; }
        public IBaseRepository<Market> Markets { get; set; }
        public IBaseRepository<Address> Addresses { get; set; }
        public IBaseRepository<ProductLine> ProductLines { get; set; }
        public IBaseRepository<Employe> Employes { get; set; }
        public IBaseRepository<EmployePassport> EmployesPassports { get; set; }
        public IBaseRepository<MarketCapital> MarketCaptails { get; set; }
        public IBaseRepository<MarketCapitalProductLine> MarketCaptailsProductLines { get; set; }

        public UnitOfWork(ApplicationDbContext context, IBaseRepository<User> users
            ,IBaseRepository<Market> markets, IBaseRepository<Address> address
            ,IBaseRepository<ProductLine> productLines,IBaseRepository<Employe> employes
            ,IBaseRepository<EmployePassport> employePassports,IBaseRepository<MarketCapital> marketCapital
            ,IBaseRepository<MarketCapitalProductLine> marketCaptailsProductsLines)
        {
            _context = context;
            Users = users;
            Markets = markets;
            Addresses = address;
            ProductLines = productLines;
            Employes = employes;
            EmployesPassports = employePassports;
            MarketCaptails = marketCapital;
            MarketCaptailsProductLines = marketCaptailsProductsLines;
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
