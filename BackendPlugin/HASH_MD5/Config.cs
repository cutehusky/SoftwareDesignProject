using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace HASH_MD5;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<HashMD5Service>();
    }

    public string ID => "10bba80b-2976-4bd7-a86f-5e97dce50bf4";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(HashMd5Controller)
    };
}