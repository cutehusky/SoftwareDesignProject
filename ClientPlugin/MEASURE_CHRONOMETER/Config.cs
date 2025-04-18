using ClientPluginTemplate;

namespace CONVERT_CHRONOMETER;

public class Config : IConfig
{
    public string ID => "45eb9a07-1a28-4e10-ad55-b12d7413bd95";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}