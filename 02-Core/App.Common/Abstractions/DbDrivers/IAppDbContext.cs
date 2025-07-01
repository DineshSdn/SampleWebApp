using System.Threading;
using System.Threading.Tasks;
using App.Common.Adapters.Data;

namespace App.Common.Abstractions.DbDrivers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IDbSetAdapter<TEntity> Set<TEntity>()
           where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDbContextTransactionAdapter BeginTransaction();
    }
}
