using System.Security.Cryptography;
using System.Text;

namespace HASH_MD5;

public class HashService
{
    public string ConvertToMD5(string input)
    {
        using var md5 = MD5.Create();
        return HashString(md5, input);
    }
    
    public string ConvertToSha3_256(string input)
    {
        using var sha256 = SHA3_256.Create();
        return HashString(sha256, input);
    }
    
    
    public string ConvertToSha3_384(string input)
    {
        using var sha384 = SHA3_384.Create();
        return HashString(sha384, input);
    }
    
    public string ConvertToSha3_512(string input)
    {
        using var sha512 = SHA3_512.Create();
        return HashString(sha512, input);
    }
    
    public string ConvertToSha2_256(string input)
    {
        using var sha256 = SHA256.Create();
        return HashString(sha256, input);
    }
    
    
    public string ConvertToSha2_384(string input)
    {
        using var sha384 = SHA384.Create();
        return HashString(sha384, input);
    }
    
    public string ConvertToSha2_512(string input)
    {
        using var sha512 = SHA512.Create();
        return HashString(sha512, input);
    }
    
    public string ConvertToSha1(string input)
    {
        using var sha1 = SHA1.Create();
        return HashString(sha1, input);
    }

    public string HashString(HashAlgorithm hash, string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = hash.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}