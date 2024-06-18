using Acef.Raisons.ApplicationCore.Entites;
using Acef.Raisons.ApplicationCore.Interfaces;
using Acef.Raisons.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Acef.Raisons.Infrastructure
{
    public class AsyncRepository<TBaseEntity> : IAsyncRepository<TBaseEntity> where TBaseEntity : BaseEntity
    {
        protected readonly RaisonContext _dbContext;

        public AsyncRepository(RaisonContext dbContext)
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
