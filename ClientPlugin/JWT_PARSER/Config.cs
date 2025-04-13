using ClientPluginTemplate;

namespace JWT_PARSER;


public class Config: IConfig
{
    public string ID => "dda68063-f056-444f-bc0e-37083517ba98";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}