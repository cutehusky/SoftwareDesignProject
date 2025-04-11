using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public interface IDynamicPageLoader
{
    public Task<DynamicPage?> LoadDynamicAssembly(string pluginId);
}