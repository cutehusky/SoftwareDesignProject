using System.ComponentModel.DataAnnotations;

namespace SoftwareDesignProject.Models.Entities;

public enum UserRoles
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
    public UserRoles UserRole { get; set; } = UserRoles.Normal;
    
    public ICollection<Plugin>? Starred { get; set; }
}