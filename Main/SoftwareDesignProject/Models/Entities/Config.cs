using System.ComponentModel.DataAnnotations;

namespace SoftwareDesignProject.Models.Entities;

public class Config: ITimestampedEntity
{
    [Key]
    public string Key { get; set; } = null!;
    public string? Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}