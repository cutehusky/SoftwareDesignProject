using ClientPluginTemplate;

namespace HASH_MD5;

public class Config: IConfig
{
    public string ID => "10bba80b-2976-4bd7-a86f-5e97dce50bf4";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}