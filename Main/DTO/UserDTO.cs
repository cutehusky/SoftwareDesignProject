namespace CommonDTO
{
    public class UserDTO
    {
        public Guid UserId { get; init; } = Guid.NewGuid();
        public string? Username { get; init; }
        public string? HashedPassword { get; init; }
        public UserRoles? UserRole { get; init; } = UserRoles.Normal;
    }
    
    public enum UserRoles
    {
        Normal,
        Premium,
        Admin
    }
}
