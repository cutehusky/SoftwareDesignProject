using ClientPluginTemplate;

namespace SECURITY_HIDEEMAIL;

public class Config : IConfig
{
    public string ID => "b73274a0-ca2c-4829-9cbb-a17ff2c6839d";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}