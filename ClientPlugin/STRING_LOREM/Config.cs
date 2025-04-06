using ClientPluginTemplate;

namespace STRING_LOREM;

public class Config : IConfig
{
    public string ID => "5c8a3c79-db48-432e-8705-cd1f06cc8bfa";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}