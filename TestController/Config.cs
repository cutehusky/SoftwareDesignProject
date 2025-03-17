using ControllerPluginTemplate;

namespace TestController;

public class Config: IConfig
{
    public string ID => "2b8bdd5e-d543-4fe1-bb84-719f31a4d068";
    public List<Type> ExportedControllers => new List<Type>()
    {
        typeof(TestController)
    };
}