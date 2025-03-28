using ClientPluginTemplate;

namespace SECURITY_PASSTEST;

public class Config : IConfig
{
    public string ID => "b9ee980a-af8b-4a80-923e-d5c822f83cc8";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}