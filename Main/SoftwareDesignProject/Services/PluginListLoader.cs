using CommonDTO;
using SoftwareDesignProject.Client.Services;

namespace SoftwareDesignProject.Services;

public class PluginListLoader: IPluginListLoader
{
    public Task<List<PluginDTO>?> GetItem()
    {
        return Task.FromResult((List<PluginDTO>?) null);
    }
}