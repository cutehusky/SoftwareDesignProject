using CommonDTO;
using MudBlazor;

namespace SoftwareDesignProject.Services;

public interface IPluginService
{
    public Task<PaginationList<PluginDTO>> GetList(int page, int pageSize,  
        string sortBy, SortDirection order, string search);

    public Task<List<PluginDTO>> GetActiveList();
    
    public Task AddPlugin(string name, string description,
        string category, bool isPremium,
        IFormFile clientDLL, IFormFile? serverDLL);

    public Task EditPlugin(PluginDTO dto);

    public Task RemovePlugin(Guid id);
    
    public Task<PluginDTO?> GetPluginById(Guid id);
    public Task UpgradePlugin(Guid pluginId, IFormFile? clientDll, IFormFile? serverDll);
}