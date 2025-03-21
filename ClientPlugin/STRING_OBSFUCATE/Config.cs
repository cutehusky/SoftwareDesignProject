using ClientPluginTemplate;

namespace STRING_OBSFUCATE;

public class Config : IConfig
{
    public string ID => "37a1eea7-567c-4cac-87eb-cece7dc68d57";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}