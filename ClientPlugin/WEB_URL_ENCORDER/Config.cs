using ClientPluginTemplate;

namespace WEB_URL_ENCORDER;

public class Config : IConfig
{
    public string ID => "bf990d36-033d-4c9a-b52f-785f316fe2f7";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}