using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace TestController;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<TestService>();
    }

    public string ID => "2b8bdd5e-d543-4fe1-bb84-719f31a4d068";
    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(TestController)
    };
}