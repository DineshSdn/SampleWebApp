using System.Collections.Generic;
using System.Threading.Tasks;
using App.Common.Adapters.Data;
using static Dapper.SqlMapper;

namespace Infrastructure.Adapters.Data
{
    public class GridReaderAdapter : IGridReaderAdapter
    {
        private readonly GridReader _gridReader;

        public GridReaderAdapter(GridReader gridReader)
        {
            _gridReader = gridReader;
        }

        public void Dispose()
            => _gridReader.Dispose();

        public IEnumerable<T> Read<T>(bool buffered = true)
            => _gridReader.Read<T>(buffered);

        public async Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true)
            => await _gridReader.ReadAsync<T>(buffered);

        public async Task<T> ReadSingleOrDefaultAsync<T>()
            => await _gridReader.ReadSingleOrDefaultAsync<T>();

        public async Task<T> ReadFirstOrDefaultAsync<T>()
            => await _gridReader.ReadFirstOrDefaultAsync<T>();

        public T ReadSingleOrDefault<T>()
            => _gridReader.ReadSingleOrDefault<T>();

        public async Task<T> ReadSingleAsync<T>()
            => await _gridReader.ReadSingleAsync<T>();
    }
}
