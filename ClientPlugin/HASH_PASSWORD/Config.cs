using ClientPluginTemplate;

namespace HASH_PASSWORD;

public class Config: IConfig
{
    public string ID => "b33c175c-94a9-4d92-b6f8-b56c6ad450b3";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}