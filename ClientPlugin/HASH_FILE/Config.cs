using ClientPluginTemplate;

namespace HASH_FILE;
public class Config: IConfig
{
    public string ID => "c34af0bf-11ad-4c51-bf2b-87d164bdf257";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}