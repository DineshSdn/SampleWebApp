using System;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Adapters.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Adapters.Data
{
    internal sealed class DbContextTransactionAdapter : IDbContextTransactionAdapter
    {
        private readonly IDbContextTransaction _instance;

        public DbContextTransactionAdapter(IDbContextTransaction instance)
        {
            _instance = instance;
        }

        public Guid TransactionId
            => _instance.TransactionId;

        public void Commit()
            => _instance.Commit();

        public async Task CommitAsync(CancellationToken cancellationToken = default)
            => await _instance.CommitAsync(cancellationToken);

        public ValueTask DisposeAsync()
            => _instance.DisposeAsync();

        public void Rollback()
            => _instance.Rollback();

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
            => await _instance.RollbackAsync(cancellationToken);

        public void Dispose()
        {
            _instance.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
