using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Базовый репозиторийдля работы с бд
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// метод добавления обьекта в бд
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> CreateAsync(TEntity entity);
        /// <summary>
        /// метод обновления обьекта
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity UpdateAsync(TEntity entity);
        /// <summary>
        /// метод удаления обьекта из бд
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteAsync(TEntity entity);
        /// <summary>
        /// метод извлечения данных из бд 
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll();
        /// <summary>
        ///  метод сохранения данных в бд
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
