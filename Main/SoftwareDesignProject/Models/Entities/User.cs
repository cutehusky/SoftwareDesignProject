using System.ComponentModel.DataAnnotations;

namespace SoftwareDesignProject.Models.Entities;

public enum UserType
{
    Normal,
    Premium,
    Admin
}

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public UserType UserType { get; set; } = UserType.Normal;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Plugin>? Starred { get; set; }
}