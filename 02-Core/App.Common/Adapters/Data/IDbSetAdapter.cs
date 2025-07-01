using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace App.Common.Adapters.Data
{
    public interface IDbSetAdapter<TEntity> : IQueryable<TEntity>
        where TEntity : class
    {
        TEntity Find(params object[] keyValues);
        ValueTask<TEntity> FindAsync(params object[] keyValues);
        ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        void AddRange(params TEntity[] entities);
        Task AddRangeAsync(params TEntity[] entities);
        void RemoveRange(params TEntity[] entities);
        void UpdateRange(params TEntity[] entities);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        void AttachRange(IEnumerable<TEntity> entities);
        void RemoveRange(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<TEntity> AsNoTracking();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
