using ClientPluginTemplate;

namespace SOURCE_GIT;

public class Config : IConfig
{
    public string ID => "2e4eb2bd-5431-420a-8fbb-0204c3bb6796";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}