using ClientPluginTemplate;

namespace MOCK_DATA;

public class Config : IConfig
{
    public string ID => "1ba34c03-382c-4a7f-a7da-09de2544ae0f";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}