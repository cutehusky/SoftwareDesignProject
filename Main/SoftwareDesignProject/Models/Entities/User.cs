using System.ComponentModel.DataAnnotations;

namespace SoftwareDesignProject.Models.Entities;

public enum UserRoles
{
    Normal,
    Premium,
    Admin
}

public class User: ITimestampedEntity
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public UserRoles UserRole { get; set; } = UserRoles.Normal;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}