using CommonDTO;

namespace SoftwareDesignProject.Repositories;

public interface IPluginRepository: IRepository<PluginDTO>
{
    public Task<List<PluginDTO>> GetActiveList();
    public Task<List<Guid>> GetStarredPluginByUserId(Guid id);
    
    public Task<bool> StarPlugin(Guid pluginId, Guid userId);
    public Task<bool> UnstarPlugin(Guid pluginId, Guid userId);
}