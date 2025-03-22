namespace HASH_FILE;

using System.Security.Cryptography;

public class HashService
{
    public async Task<string> ConvertToMD5(Stream stream, Func<byte[], string> converter)
    {
        using var md5 = MD5.Create();
        return await HashFile(md5, stream, converter);
    }
    
    public async Task<string>  ConvertToSha3_256(Stream stream, Func<byte[], string> converter)
    {
        using var sha256 = SHA3_256.Create();
        return await HashFile(sha256, stream, converter);
    }
    
    public async Task<string>  ConvertToSha3_384(Stream stream, Func<byte[], string> converter)
    {
        using var sha384 = SHA3_384.Create();
        return await HashFile(sha384, stream, converter);
    }
    
    public async Task<string>  ConvertToSha3_512(Stream stream, Func<byte[], string> converter)
    {
        using var sha512 = SHA3_512.Create();
        return await HashFile(sha512, stream, converter);
    }
    
    public async Task<string>  ConvertToSha2_256(Stream stream, Func<byte[], string> converter)
    {
        using var sha256 = SHA256.Create();
        return await HashFile(sha256, stream, converter);
    }
    
    
    public async Task<string>  ConvertToSha2_384(Stream stream, Func<byte[], string> converter)
    {
        using var sha384 = SHA384.Create();
        return await HashFile(sha384, stream, converter);
    }
    
    public async Task<string>  ConvertToSha2_512(Stream stream, Func<byte[], string> converter)
    {
        using var sha512 = SHA512.Create();
        return await HashFile(sha512, stream, converter);
    }
    
    public async Task<string> ConvertToSha1(Stream stream, Func<byte[], string> converter)
    {
        using var sha1 = SHA1.Create();
        return await HashFile(sha1, stream, converter);
    }

    public async Task<string> HashFile(HashAlgorithm hash, Stream stream, Func<byte[], string> converter)
    {
        byte[] hashBytes = await hash.ComputeHashAsync(stream);
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