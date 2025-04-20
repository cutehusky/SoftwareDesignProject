using ClientPluginTemplate;

namespace CONVERT_BANDWIDTH;

public class Config : IConfig
{
    public string ID => "771eb870-4afe-49d6-b2ce-4ea89cad97df";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}