using Acef.Reasons.ApplicationCore.Entities;
using Acef.Reasons.ApplicationCore.Interfaces;
using Acef.Reasons.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Acef.Reasons.Infrastructure
{
    public class AsyncRepository<TBaseEntity> : IAsyncRepository<TBaseEntity> where TBaseEntity : BaseEntity
    {
        protected readonly ReasonsDbContext _dbContext;

        public AsyncRepository(ReasonsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TBaseEntity entity)
        {
            await _dbContext.Set<TBaseEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TBaseEntity entity)
        {
            _dbContext.Set<TBaseEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(TBaseEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TBaseEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TBaseEntity>().AsNoTracking().SingleOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<TBaseEntity>> ListAsync()
        {
            return await _dbContext.Set<TBaseEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TBaseEntity>> ListAsync(Expression<Func<TBaseEntity, bool>> predicate)
        {
            return await _dbContext.Set<TBaseEntity>()
                .Where(predicate)
                .ToListAsync();
        }
    }
}
