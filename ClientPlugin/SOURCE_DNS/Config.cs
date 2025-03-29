using ClientPluginTemplate;

namespace SOURCE_DNS;

public class Config : IConfig
{
    public string ID => "3da5c205-3e5d-4de1-a441-ab8b215fae73";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}