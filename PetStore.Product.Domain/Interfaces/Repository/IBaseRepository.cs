namespace PetStore.Products.Domain.Interfaces.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> CreateAsync(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity UpdateAsync(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteAsync(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
