using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Common.Adapters.Data
{
    public interface IGridReaderAdapter : IDisposable
    {
        public IEnumerable<T> Read<T>(bool buffered = true);
        Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true);
        T ReadSingleOrDefault<T>();
        Task<T> ReadSingleAsync<T>();
        Task<T> ReadSingleOrDefaultAsync<T>();
        Task<T> ReadFirstOrDefaultAsync<T>();
    }
}
