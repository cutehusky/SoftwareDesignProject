using SoftwareDesignProject.Client.Models;
using SoftwareDesignProject.Client.Services;

namespace SoftwareDesignProject.Services;

public class PluginListLoader: IPluginListLoader
{
    public Task<List<PluginItem>?> GetItem()
    {
        return Task.FromResult((List<PluginItem>?) null);
    }
}