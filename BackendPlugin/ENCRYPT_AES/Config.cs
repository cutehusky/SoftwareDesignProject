using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace ENCRYPT_AES;


public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<EncryptService>();
    }

    public string ID => "7c22a439-fe40-4935-934b-e93804484289";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(EncryptController)
    };
}