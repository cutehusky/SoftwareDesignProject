using CommonDTO;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignProject.Models.DTOMapper;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Repositories;

public class PluginRepository
{
    private readonly AppDbContext _dbContext;

    public PluginRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PluginDTO>> GetAll()
    {
        return await _dbContext.Plugins.Select(plugin => new PluginDTOMapper().ConvertTo(plugin))
            .ToListAsync();
    }

    public async Task<bool> Add(PluginDTO pluginDto)
    {
        _dbContext.Plugins.Add(new PluginDTOMapper().ConvertFrom(pluginDto));
        var rowAffected = await _dbContext.SaveChangesAsync();
        return rowAffected > 0;
    }

    public async Task<bool> Remove(Guid id)
    {
        var rowAffected = await _dbContext.Plugins.Where((plugin => plugin.PluginId == id))
            .ExecuteDeleteAsync();
        return rowAffected > 0;
    }

    public async Task<bool> Update(PluginDTO dto)
    {
        var target = await _dbContext.Plugins.Where(plugin => plugin.PluginId == dto.PluginId).FirstOrDefaultAsync();
        if (target == null)
            return false;
        new PluginDTOMapper().CopyToEntity(target, dto);
        var rowAffected = await _dbContext.SaveChangesAsync();
        return rowAffected > 0;
    }
}