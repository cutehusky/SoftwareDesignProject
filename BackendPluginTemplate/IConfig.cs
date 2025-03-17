using Microsoft.Extensions.DependencyInjection;

namespace BackendPluginTemplate;

public interface IConfig
{
    public string ID { get; }
    public List<Type> ExportedControllers { get; }
    public void RegisterService(IServiceCollection sc);
}