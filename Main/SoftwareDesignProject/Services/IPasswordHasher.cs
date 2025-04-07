// IPasswordHasher.cs
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}

// BCryptPasswordHasher.cs
public class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string hashedPassword, string providedPassword) =>
        BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
}