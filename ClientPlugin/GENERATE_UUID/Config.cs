using ClientPluginTemplate;

namespace UUID;

public class Config: IConfig
{
    public string ID => "7eab8161-81c9-41f2-a806-208ad89d0ca7";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}