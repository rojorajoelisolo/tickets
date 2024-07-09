using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Contexts;
using tickets.Interfaces.Repositories;

namespace tickets.Infrastructures.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly TicketsContext _ticketsContext;

        public BaseRepository(TicketsContext ticketsContext)
        {
            _ticketsContext = ticketsContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _ticketsContext.Set<TEntity>().AddAsync(entity);
            await _ticketsContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _ticketsContext.Set<TEntity>().Remove(entity);
            return await _ticketsContext.SaveChangesAsync() != 0;
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await _ticketsContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id) => await _ticketsContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _ticketsContext.Update(entity);
            await _ticketsContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _ticketsContext.Set<TEntity>().AddRangeAsync(entities);
            await _ticketsContext.SaveChangesAsync();
            return entities;
        }
    }
}
