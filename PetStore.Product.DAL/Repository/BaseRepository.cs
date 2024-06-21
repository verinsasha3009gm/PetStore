using PetStore.Products.Domain.Interfaces;
using PetStore.Products.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.DAL.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if(entity==null) throw new NotImplementedException();
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void DeleteAsync(TEntity entity)
        {
           if(entity == null) throw new NotImplementedException();
           _context.Remove(entity);
        }   

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public TEntity UpdateAsync(TEntity entity)
        {
            if(entity==null) throw new NotImplementedException();
            _context.Update(entity);
            return entity;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
