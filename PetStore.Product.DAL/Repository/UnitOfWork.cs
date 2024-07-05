using Microsoft.EntityFrameworkCore.Storage;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Interfaces.Repository;

namespace PetStore.Products.DAL.Repository
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Description> Descriptions { get; set; }
        public IBaseRepository<ProductPassport> ProductPassports { get; set; }
        public IBaseRepository<Product> Products { get; set; }
        public IBaseRepository<Teg> Tegs { get; set; }
        public IBaseRepository<Category> Categories { get; set; }
        public IBaseRepository<ProductTeg> ProductTegs { get; set; }
        public UnitOfWork(ApplicationDbContext context, IBaseRepository<Product> products, IBaseRepository<Teg> tegs,
            IBaseRepository<ProductTeg> productTegs,IBaseRepository<ProductPassport>productPassports
            ,IBaseRepository<Description> descriptions,IBaseRepository<Category> categories)
        {
            _context = context;
            ProductTegs = productTegs;
            Products = products;
            Tegs = tegs;
            ProductPassports = productPassports;
            Descriptions = descriptions;
            Categories = categories;
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
