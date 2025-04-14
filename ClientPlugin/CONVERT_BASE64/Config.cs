using ClientPluginTemplate;

namespace CONVERT_BASE64;

public class Config: IConfig
{
    public string ID => "5fa1d505-c776-489b-8560-b27d7a45b98b";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}