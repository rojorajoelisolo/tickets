namespace tickets.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IList<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
    }
}