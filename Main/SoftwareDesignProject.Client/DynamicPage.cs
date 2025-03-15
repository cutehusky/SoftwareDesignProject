using System.Reflection;

namespace SoftwareDesignProject.Client;

public class DynamicPage
{
    public Assembly? PluginAssembly { get; set; }

    public Type? GetComponentType(string typeName)
    {
        return PluginAssembly?.GetType(typeName);
    }
}