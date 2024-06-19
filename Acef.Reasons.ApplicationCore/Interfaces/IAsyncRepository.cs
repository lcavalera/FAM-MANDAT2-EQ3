using Acef.Reasons.ApplicationCore.Entities;
using System.Linq.Expressions;

namespace Acef.Reasons.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<TBaseEntity> where TBaseEntity : BaseEntity
    {
        Task<TBaseEntity> GetByIdAsync(int id);
        Task<IEnumerable<TBaseEntity>> ListAsync();
        Task<IEnumerable<TBaseEntity>> ListAsync(Expression<Func<TBaseEntity, bool>> predicate);
        Task AddAsync(TBaseEntity entity);
        Task DeleteAsync(TBaseEntity entity);
        Task EditAsync(TBaseEntity entity);
    }
}
