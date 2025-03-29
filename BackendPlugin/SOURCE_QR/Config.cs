using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace SOURCE_QR;

public class Config : IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<Service>();
    }

    public string ID => "f271ed95-1975-4401-9053-118404d0917b";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(SourceQrController)
    };
}