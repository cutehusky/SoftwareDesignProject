using ClientPluginTemplate;

namespace CONVERT_TEMPERATURE;

public class Config : IConfig
{
    public string ID => "e1a4c6d4-654e-4c44-b8c3-bd246c87bb71";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}