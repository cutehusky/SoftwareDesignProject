using ClientPluginTemplate;

namespace RANDOM_TOKEN;

public class Config: IConfig
{
    public string ID => "9badce97-d32f-4215-9f6d-fb340957c238";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}