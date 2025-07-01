namespace App.Common.Adapters.Data
{
    public interface IObjectAdapterService
    {
        TDestination Adapt<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
