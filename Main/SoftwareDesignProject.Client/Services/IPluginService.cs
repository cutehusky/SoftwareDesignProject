using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public interface IPluginService
{
    public Task<List<PluginDTO>?> GetList();
    public Task Add(PluginUploadData data);
    public Task Edit(PluginDTO dto);
    public Task Upgrade(PluginUpgradeData data);
    public Task Remove(Guid id);
}