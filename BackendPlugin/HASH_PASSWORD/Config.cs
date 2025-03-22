using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace HASH_PASSWORD;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<HashService>();
    }

    public string ID => "b33c175c-94a9-4d92-b6f8-b56c6ad450b3";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(HashController)
    };
}