using Microsoft.Extensions.DependencyInjection;

namespace BackendPluginTemplate;

public interface IDynamicServiceProvider: IServiceProvider, IServiceScopeFactory
{
    public T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }
}