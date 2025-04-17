using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace ENCRYPT_RSA;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<EncryptService>();
    }

    public string ID => "86562091-5483-4f5f-bc77-df763ad7bf3a";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(EncryptController)
    };
}