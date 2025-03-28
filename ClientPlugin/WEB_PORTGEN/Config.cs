using ClientPluginTemplate;

namespace WEB_PORTGEN;

public class Config : IConfig
{
    public string ID => "2e284ab1-aad9-4913-9db6-70fcb2b80d26";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}