using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public interface IPluginListLoader
{
    public Task<List<PluginItem>?> GetItem();
}