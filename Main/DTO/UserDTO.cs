namespace CommonDTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public UserType UserType { get; set; } = UserType.Normal;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
    public enum UserType
    {
        Normal,
        Premium,
        Admin
    }
}
