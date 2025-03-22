using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace HASH_FILE;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<HashService>();
    }

    public string ID => "c34af0bf-11ad-4c51-bf2b-87d164bdf257";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(HashController)
    };
}