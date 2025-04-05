using CommonDTO;
using SoftwareDesignProject.Models.Entities;

namespace SoftwareDesignProject.Models.DTOMapper;

public class PluginDTOMapper: IDTOMapper<Plugin, PluginDTO>
{
    public PluginDTO ConvertTo(Plugin from)
    {
        return new PluginDTO()
        {
            PluginId = from.PluginId,
            Name = from.Name,
            Description = from.Description,
            Category = from.Category,
            IsEnabled = from.IsEnabled,
            IsPremium = from.IsPremium
        };
    }

    public Plugin ConvertFrom(PluginDTO from)
    {
        return new Plugin()
        {
            PluginId = from.PluginId,
            Name = string.IsNullOrEmpty(from.Name) ? from.PluginId.ToString() : from.Name,
            Description = from.Description ?? "",
            Category = from.Category ?? Plugin.DefaultCategory,
            IsEnabled = from.IsEnabled ?? true,
            IsPremium = from.IsPremium ?? false
        };
    }

    public void CopyToEntity(Plugin target, PluginDTO source)
    {
        if (source.Name != null)
            target.Name = source.Name;
        if (source.IsPremium != null)
            target.IsPremium = (bool)source.IsPremium;
        if (source.Category != null)
            target.Category = source.Category;
        if (source.Description != null)
            target.Description = source.Description;
        if (source.IsEnabled != null)
            target.IsEnabled = (bool)source.IsEnabled;
    }
}