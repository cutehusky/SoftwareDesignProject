using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace HASH_SHA;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<HashShaService>();
    }

    public string ID => "fca4e345-3281-4099-9eb1-f58320ae94b0";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(HashShaController)
    };
}