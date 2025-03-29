using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace SOURCE_DNS;

public class Config : IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<Service>();
    }

    public string ID => "3da5c205-3e5d-4de1-a441-ab8b215fae73";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(SourceDNSController)
    };
}