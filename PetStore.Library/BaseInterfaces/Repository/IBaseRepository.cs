using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.BaseInterfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public Task<TEntity> CreateAsync(TEntity entity);
        public TEntity UpdateAsync(TEntity entity);
        public void DeleteAsync(TEntity entity);
        public IQueryable<TEntity> GetAll();
    }
}
