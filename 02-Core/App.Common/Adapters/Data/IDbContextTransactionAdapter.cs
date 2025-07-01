using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Common.Adapters.Data
{
    public interface IDbContextTransactionAdapter : IDisposable, IAsyncDisposable
    {
        Guid TransactionId { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default);
        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
