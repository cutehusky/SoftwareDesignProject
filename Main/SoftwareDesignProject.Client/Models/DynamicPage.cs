using System.Reflection;

namespace SoftwareDesignProject.Client.Models;

public class DynamicPage
{
    public Assembly? PluginAssembly { get; set; }
    public Type? EntryPoint { get; set; }
}