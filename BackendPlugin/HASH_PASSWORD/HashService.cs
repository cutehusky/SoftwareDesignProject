namespace HASH_PASSWORD;

public class HashService
{
    public string HashPassword(string password, int workFactor)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}