using ClientPluginTemplate;

namespace WEB_URL_PARSER;

public class Config : IConfig
{
    public string ID => "26cfe2fd-d31a-4f39-8d85-69386b1736df";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}