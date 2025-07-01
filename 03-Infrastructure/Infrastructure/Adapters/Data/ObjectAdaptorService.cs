using App.Common.Adapters.Data;
using App.Common.Attributes;
using Mapster;
using MapsterMapper;

namespace Infrastructure.Adapters.Data
{
    [TransientService]
    public sealed class ObjectAdaptorService : IObjectAdapterService
    {
        private readonly IMapper _mapper;

        public ObjectAdaptorService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Adapt<TDestination>(object source)
        {
            return source.Adapt<TDestination>();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
