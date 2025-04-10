namespace SoftwareDesignProject.Models.Entities;

public interface ITimestampedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}