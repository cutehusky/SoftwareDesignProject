using ClientPluginTemplate;

namespace ENCRYPT_RSA;

public class Config: IConfig
{
    public string ID => "86562091-5483-4f5f-bc77-df763ad7bf3a";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}