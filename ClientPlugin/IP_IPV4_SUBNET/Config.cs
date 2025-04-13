using ClientPluginTemplate;

namespace IP_IPV4_SUBNET;

public class Config : IConfig
{
    public string ID => "0ee17453-ae55-4256-999f-6de9d4c27efd";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}