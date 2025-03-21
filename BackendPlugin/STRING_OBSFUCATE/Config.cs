using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace STRING_OBSFUCATE;

public class Config : IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<Service>();
    }

    public string ID => "37a1eea7-567c-4cac-87eb-cece7dc68d57";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(StringObsfucateController)
    };
}