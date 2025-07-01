namespace App.Common.Adapters.Json
{
    public interface IJsonSerializerAdapter
    {
        string SerializeObject(object value);
        TModel DeserializeObject<TModel>(string value);
    }
}
