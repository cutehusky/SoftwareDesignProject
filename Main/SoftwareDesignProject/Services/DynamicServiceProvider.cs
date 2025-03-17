using BackendPluginTemplate;

namespace SoftwareDesignProject.Services;

public class DynamicServiceProvider: IDynamicServiceProvider 
{
    private readonly DynamicPluginManager _pluginManager;
    
    public DynamicServiceProvider(DynamicPluginManager pluginManager)
    {
        _pluginManager = pluginManager;
    }

    public T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }

    public IServiceScope CreateScope()
    {
        return _pluginManager.GetServiceProvider().CreateScope();
    }

    public object? GetService(Type serviceType)
    {
        return _pluginManager.GetServiceProvider().GetService(serviceType);
    }
}