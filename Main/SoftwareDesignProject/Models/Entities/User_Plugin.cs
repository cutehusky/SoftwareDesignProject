using Microsoft.EntityFrameworkCore;

namespace SoftwareDesignProject.Models.Entities;

[PrimaryKey(nameof(UserId), nameof(PluginId))]
public class User_Plugin: ITimestampedEntity
{
    public User? User { get; set; } = null;
    public Guid UserId { get; set; }
    public Plugin? Plugin { get; set; } = null;
    public Guid PluginId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}