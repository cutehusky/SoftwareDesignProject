using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace CONVERT_TEMPERATURE;

public class Config : IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<Service>();
    }

    public string ID => "e1a4c6d4-654e-4c44-b8c3-bd246c87bb71";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(ConvertTemperatureController)
    };
}