using ClientPluginTemplate;

namespace IP_IPV4_ADDRESS_CONVERTER;

public class Config : IConfig
{
    public string ID => "e639b911-e8ed-403d-af54-e3f0da740b2a";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}