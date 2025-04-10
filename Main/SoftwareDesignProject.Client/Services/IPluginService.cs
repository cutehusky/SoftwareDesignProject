using CommonDTO;
using MudBlazor;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public interface IPluginService
{
    public Task<PaginationList<PluginDTO>> GetList(int page, int pageSize,
        string sortBy, SortDirection order, string search,
        CancellationToken cancellationToken);
    public Task Add(PluginUploadData data);
    public Task Edit(PluginDTO dto);
    public Task Upgrade(PluginUpgradeData data);
    public Task Remove(Guid id);
    
    public Task StarPlugin(Guid pluginId);
    public Task UnstarPlugin(Guid pluginId);
}