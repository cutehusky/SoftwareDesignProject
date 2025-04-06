using CommonDTO;
using SoftwareDesignProject.Models.DTOMapper;

namespace SoftwareDesignProject.Repositories;

public interface IPluginRepository: IRepository<PluginDTO>
{
    public Task<List<PluginDTO>> GetActiveList();
}