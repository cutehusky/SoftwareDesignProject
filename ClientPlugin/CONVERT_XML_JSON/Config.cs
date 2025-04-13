using ClientPluginTemplate;

namespace CONVERT_XML_JSON;


public class Config: IConfig
{
    public string ID => "52990b50-978c-44d7-9c49-8832ff2f9be9";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}