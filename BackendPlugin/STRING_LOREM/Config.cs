using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace STRING_LOREM;

public class Config : IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<StringLoremService>();
    }

    public string ID => "5c8a3c79-db48-432e-8705-cd1f06cc8bfa";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(StringLoremController)
    };
}