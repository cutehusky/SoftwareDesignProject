using CommonDTO;

namespace SoftwareDesignProject.Services;

public interface IPluginService
{
    public Task<List<PluginDTO>> GetList();

    public Task<List<PluginDTO>> GetActiveList();
    
    public Task AddPlugin(string name, string description,
        string category, bool isPremium,
        IFormFile clientDLL, IFormFile? serverDLL);

    public Task EditPlugin(PluginDTO dto);

    public Task RemovePlugin(Guid id);
    
    public Task<PluginDTO?> GetPluginById(Guid id);
    public Task UpgradePlugin(Guid pluginId, IFormFile? clientDll, IFormFile? serverDll);
}