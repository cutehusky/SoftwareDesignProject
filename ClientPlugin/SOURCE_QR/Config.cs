using ClientPluginTemplate;

namespace SOURCE_QR;

public class Config : IConfig
{
    public string ID => "f271ed95-1975-4401-9053-118404d0917b";
    public Type EntryPoint => typeof(MainComponent);
    public static string apiEndPoint;
    public string APIEndPoint
    {
        set => apiEndPoint = value;
    }
}