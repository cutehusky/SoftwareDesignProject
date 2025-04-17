using ClientPluginTemplate;

namespace ENCRYPT_RSA_KEY;

public class Config: IConfig
{
    public string ID => "7d89c6ac-6638-49ee-9012-d4c8a1dc8b7e";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;

    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}