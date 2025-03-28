namespace CommonDTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string? Username { get; set; }
        public string? HashedPassword { get; set; }
        public UserRoles? UserRole { get; set; } = UserRoles.Normal;
    }
    
    public enum UserRoles
    {
        Normal,
        Premium,
        Admin
    }
}
