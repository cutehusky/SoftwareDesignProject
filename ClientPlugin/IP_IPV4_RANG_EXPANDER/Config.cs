using ClientPluginTemplate;

namespace IP_IPV4_RANG_EXPANDER;

public class Config : IConfig
{
    public string ID => "5bc09e33-8c2c-4ec5-8071-6080fe8ebfdf";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}