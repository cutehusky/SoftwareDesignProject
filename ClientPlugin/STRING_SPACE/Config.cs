using ClientPluginTemplate;

namespace STRING_SPACE;

public class Config : IConfig
{
    public string ID => "705d679e-aee6-4dd7-9a91-6a261f5f77ee";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}