using PluginTemplate;

namespace RazorClassLibrary_test;

public class Config: IConfig
{
    public string ID => "2b8bdd5e-d543-4fe1-bb84-719f31a4d068";
    public Type EntryPoint => typeof(Component1);
    
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }

    public static string apiEndPoint = "";
}