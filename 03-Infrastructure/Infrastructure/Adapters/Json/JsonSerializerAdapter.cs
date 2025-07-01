using App.Common.Adapters.Json;
using App.Common.Attributes;
using Newtonsoft.Json;

namespace Infrastructure.Adapters.Json
{
    [TransientService]
    public class JsonSerializerAdapter : IJsonSerializerAdapter
    {
        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public TModel DeserializeObject<TModel>(string value)
        {
            return JsonConvert.DeserializeObject<TModel>(value);
        }
    }
}
