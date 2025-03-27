using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public interface IPluginListLoader
{
    public Task<List<PluginDTO>?> GetItem();
}