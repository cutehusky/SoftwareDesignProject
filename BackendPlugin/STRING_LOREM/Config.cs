using BackendPluginTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace STRING_LOREM;

public class Config : IConfig
{
    public void RegisterService(IServiceCollection sc)
    {
        sc.AddSingleton<StringLoremService>();
    }

    public string ID => "fckjs345-3281-4099-9eb1-f52498ae94b0";

    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(StringLoremController)
    };
}