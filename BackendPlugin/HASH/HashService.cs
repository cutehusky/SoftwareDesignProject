using System.Security.Cryptography;
using System.Text;

namespace HASH;

public class HashService
{
    public string ConvertToMD5(string input, Func<byte[], string> converter)
    {
        using var md5 = MD5.Create();
        return HashString(md5, input, converter);
    }
    
    public string ConvertToSha3_256(string input, Func<byte[], string> converter)
    {
        using var sha256 = SHA3_256.Create();
        return HashString(sha256, input, converter);
    }
    
    
    public string ConvertToSha3_384(string input, Func<byte[], string> converter)
    {
        using var sha384 = SHA3_384.Create();
        return HashString(sha384, input, converter);
    }
    
    public string ConvertToSha3_512(string input, Func<byte[], string> converter)
    {
        using var sha512 = SHA3_512.Create();
        return HashString(sha512, input, converter);
    }
    
    public string ConvertToSha2_256(string input, Func<byte[], string> converter)
    {
        using var sha256 = SHA256.Create();
        return HashString(sha256, input, converter);
    }
    
    
    public string ConvertToSha2_384(string input, Func<byte[], string> converter)
    {
        using var sha384 = SHA384.Create();
        return HashString(sha384, input, converter);
    }
    
    public string ConvertToSha2_512(string input, Func<byte[], string> converter)
    {
        using var sha512 = SHA512.Create();
        return HashString(sha512, input, converter);
    }
    
    public string ConvertToSha1(string input, Func<byte[], string> converter)
    {
        using var sha1 = SHA1.Create();
        return HashString(sha1, input, converter);
    }

    public string HashString(HashAlgorithm hash, string input, Func<byte[], string> converter)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = hash.ComputeHash(bytes);
        return converter.Invoke(hashBytes);
    }

    public static string BytesToBinString(byte[] byteArray)
    {
        return string.Join("", Array.ConvertAll(byteArray, b => Convert.ToString(b, 2).PadLeft(8, '0')));
    }

    public static string BytesToBase64URL(byte[] byteArray)
    {
        return Convert.ToBase64String(byteArray).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
    
    public static string BytesToBase64(byte[] byteArray)
    {
        return Convert.ToBase64String(byteArray);
    }

    public static string BytesToHexString(byte[] byteArray)
    {
        return BitConverter.ToString(byteArray).Replace("-", "").ToUpper();
    }
}