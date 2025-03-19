using ClientPluginTemplate;

namespace STRING_LOREM;

public class Config : IConfig
{
    public string ID => "fckjs345-3281-4099-9eb1-f52498ae94b0";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}