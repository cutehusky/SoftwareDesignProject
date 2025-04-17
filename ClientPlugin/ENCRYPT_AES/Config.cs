using ClientPluginTemplate;

namespace ENCRYPT_AES;

public class Config: IConfig
{
    public string ID => "7c22a439-fe40-4935-934b-e93804484289";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}