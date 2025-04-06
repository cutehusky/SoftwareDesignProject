using CommonDTO;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SoftwareDesignProject.Models.DTOMapper;
using SoftwareDesignProject.Models.Entities;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Repositories;

public class PluginRepository: IPluginRepository
{
    private readonly AppDbContext _dbContext;

    public PluginRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationList<PluginDTO>> GetAll(int page, int pageSize,
        string sortBy, SortDirection order, string search)
    {
        var totalCount = await _dbContext.Plugins.CountAsync();
        var plugins = sortBy switch
        {
            "Name" => order == SortDirection.Descending
                ? _dbContext.Plugins.OrderByDescending(plugin => plugin.Name)
                : _dbContext.Plugins.OrderBy(plugin => plugin.Name),
            "Category" => order == SortDirection.Descending
                ? _dbContext.Plugins.OrderByDescending(plugin => plugin.Category)
                : _dbContext.Plugins.OrderBy(plugin => plugin.Category),
            "IsPremium" => order == SortDirection.Descending
                ? _dbContext.Plugins.OrderByDescending(plugin => plugin.IsPremium)
                : _dbContext.Plugins.OrderBy(plugin => plugin.IsPremium),
            "IsEnabled" => order == SortDirection.Descending
                ? _dbContext.Plugins.OrderByDescending(plugin => plugin.IsEnabled)
                    : _dbContext.Plugins.OrderBy(plugin => plugin.IsEnabled),
            _ => order == SortDirection.Descending
                ? _dbContext.Plugins.OrderByDescending(plugin => plugin.PluginId)
                    : _dbContext.Plugins.OrderBy(plugin => plugin.PluginId)
        };
        var item = await plugins
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(plugin => new PluginDTOMapper().ConvertTo(plugin))
            .ToListAsync();
        
        return new PaginationList<PluginDTO>()
        {
            Items = item,
            TotalCount = totalCount
        };
    }
    
    public async Task<List<PluginDTO>> GetActiveList()
    {
        return await _dbContext.Plugins.Where(plugin => plugin.IsEnabled)
            .Select(plugin => new PluginDTOMapper().ConvertTo(plugin))
            .ToListAsync();
    }
    
    public async Task<PluginDTO?> GetById(Guid id)
    {
        return await _dbContext.Plugins.Where(plugin => plugin.PluginId == id)
            .Select(plugin => new PluginDTOMapper().ConvertTo(plugin))
            .FirstOrDefaultAsync();
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