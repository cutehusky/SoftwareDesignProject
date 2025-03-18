using ClientPluginTemplate;

namespace HASH_SHA1;

public class Config: IConfig
{
    public string ID => "fca4e345-3281-4099-9eb1-f58320ae94b0";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}