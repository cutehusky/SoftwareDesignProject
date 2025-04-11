using System.Reflection;
using CommonDTO;

namespace SoftwareDesignProject.Client.Models;

public class DynamicPage
{
    public PluginDTO Plugin { get; set; } = new();
    public Assembly? PluginAssembly { get; set; }
    public Type? EntryPoint { get; set; }
}