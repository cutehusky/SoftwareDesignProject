using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace ENCRYPT_RSA_KEY;


public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<RSAService>();
    }

    public string ID => "7d89c6ac-6638-49ee-9012-d4c8a1dc8b7e";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(EncryptController)
    };
}