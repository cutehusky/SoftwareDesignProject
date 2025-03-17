using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace TestController2;

public class Config: IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        
    }

    public string ID => "a89eb8f6-e3dd-469d-8ee6-08f9e027fed2";
    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(TestController)
    };
}