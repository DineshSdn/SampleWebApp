using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Adapters.Data;
using App.Common.Attributes;
using Dapper;

namespace Infrastructure.Adapters.Data
{
    [TransientService]
    public class QueryAdapter : IQueryAdapter
    {
        public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
            => await cnn.QueryAsync<T>(GetCommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken));

        public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
            => await cnn.QueryFirstOrDefaultAsync<T>(GetCommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken));

        public async Task<IGridReaderAdapter> QueryMultipleAsync(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
            => new GridReaderAdapter(await cnn.QueryMultipleAsync(GetCommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken)));

        public async Task<T> ExecuteScalarAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
            => await cnn.ExecuteScalarAsync<T>(GetCommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken));

        public async Task<int> ExecuteAsync(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
            => await cnn.ExecuteAsync(GetCommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken));

        private static CommandDefinition GetCommandDefinition(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
        {
            return new CommandDefinition
            (
                sql,
                parameters: param,
                commandTimeout: commandTimeout,
                commandType: commandType,
                cancellationToken: cancellationToken
            );
        }
    }
}
