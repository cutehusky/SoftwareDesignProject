using ClientPluginTemplate;

namespace SECURITY_PASSGEN;

public class Config : IConfig
{
    public string ID => "024220f8-72f5-4520-955d-0ea26ecdc3af";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}