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
            Name = from.Name ?? "",
            Description = from.Description ?? "",
            Category = from.Category ?? Plugin.DefaultCategory,
            IsEnabled = from.IsEnabled ?? true,
            IsPremium = from.IsPremium ?? false
        };
    }
}