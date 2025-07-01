using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Adapters.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Data
{
    internal sealed class DbSetAdapter<TEntity> : IDbSetAdapter<TEntity>
       where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly IQueryable<TEntity> _query;

        public DbSetAdapter(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet;
            _query = dbSet.AsQueryable();
        }

        public Type ElementType
            => _query.ElementType;

        public Expression Expression
            => _query.Expression;

        public IQueryProvider Provider
            => _query.Provider;

        public void Add(TEntity entity)
            => _dbSet.Add(entity);

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => await _dbSet.AddAsync(entity, cancellationToken);

        public void AddRange(params TEntity[] entities)
            => _dbSet.AddRange(entities);

        public void AddRange(IEnumerable<TEntity> entities)
            => _dbSet.AddRange(entities);

        public async Task AddRangeAsync(params TEntity[] entities)
            => await _dbSet.AddRangeAsync(entities);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => await _dbSet.AddRangeAsync(entities, cancellationToken);

        public void AttachRange(IEnumerable<TEntity> entities)
            => _dbSet.AttachRange(entities);

        public TEntity Find(params object[] keyValues)
            => _dbSet.Find(keyValues);

        public ValueTask<TEntity> FindAsync(params object[] keyValues)
            => _dbSet.FindAsync(keyValues);

        public ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
            => _dbSet.FindAsync(keyValues, cancellationToken);

        public IEnumerator<TEntity> GetEnumerator()
            => _query.GetEnumerator();

        public void Remove(TEntity entity)
            => _dbSet.Remove(entity);

        public void RemoveRange(params TEntity[] entities)
            => _dbSet.RemoveRange(entities);

        public void RemoveRange(IEnumerable<TEntity> entities)
            => _dbSet.RemoveRange(entities);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void UpdateRange(params TEntity[] entities)
            => _dbSet.UpdateRange(entities);

        public void UpdateRange(IEnumerable<TEntity> entities)
            => _dbSet.UpdateRange(entities);

        IEnumerator IEnumerable.GetEnumerator()
            => _query.GetEnumerator();

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        public IQueryable<TEntity> AsNoTracking()
            => _dbSet.AsNoTracking();

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(predicate, cancellationToken);

        public async Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
            => await _dbSet.ToListAsync(cancellationToken);

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
           => await _dbSet.CountAsync(predicate, cancellationToken);
    }
}
