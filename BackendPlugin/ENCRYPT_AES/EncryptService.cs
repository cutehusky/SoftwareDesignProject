using System.Security.Cryptography;
using System.Text;

namespace ENCRYPT_AES;

public class EncryptService
{
    private static byte[] CreateKey_AES(string key)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
    }
    
    private static byte[] CreateKey_TripleDES(string key)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(key)).Take(24).ToArray();
    }

    public string Encrypt_TripleDES(string plainText, string key)
    {
        byte[] keyBytes = CreateKey_TripleDES(key);

        using TripleDES tripleDES = TripleDES.Create();
        tripleDES.Key = keyBytes;
        tripleDES.Mode = CipherMode.CBC;
        tripleDES.Padding = PaddingMode.PKCS7;
        tripleDES.GenerateIV(); // Random IV

        byte[] iv = tripleDES.IV;

        using MemoryStream ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length); // Prepend IV

        using (CryptoStream cs = new CryptoStream(ms, tripleDES.CreateEncryptor(), CryptoStreamMode.Write))
        using (StreamWriter sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
            sw.Flush();
            cs.FlushFinalBlock();
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt_TripleDES(string cipherText, string key)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] keyBytes = CreateKey_TripleDES(key);

        using TripleDES tripleDES = TripleDES.Create();
        tripleDES.Key = keyBytes;
        tripleDES.Mode = CipherMode.CBC;
        tripleDES.Padding = PaddingMode.PKCS7;

        int ivSize = tripleDES.BlockSize / 8; // 8 bytes for 3DES
        byte[] iv = new byte[ivSize];
        Array.Copy(cipherBytes, 0, iv, 0, ivSize);
        tripleDES.IV = iv;

        using MemoryStream ms = new MemoryStream(cipherBytes, ivSize, cipherBytes.Length - ivSize);
        using CryptoStream cs = new CryptoStream(ms, tripleDES.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
    
    public string Encrypt_AES(string value, string key)
    {
        using Aes aes = Aes.Create();
        aes.Key = CreateKey_AES(key);
        aes.GenerateIV();
        byte[] iv = aes.IV;
        Console.WriteLine($"AES Block Size: {aes.BlockSize}");
        Console.WriteLine($"Key: {Convert.ToBase64String(aes.Key)}");
        Console.WriteLine($"IV: {Convert.ToBase64String(iv)}");
        using MemoryStream ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length);

        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        using (StreamWriter sw = new StreamWriter(cs))
        {
            sw.Write(value);
            sw.Flush(); 
            cs.FlushFinalBlock(); 
        }
        return Convert.ToBase64String(ms.ToArray());
    }
    
    public string Decrypt_AES(string cipherText, string key)
    {
        byte[] bytes = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = CreateKey_AES(key);
        byte[] iv = new byte[aes.BlockSize / 8];
        Array.Copy(bytes, iv, iv.Length);
        aes.IV = iv;
        Console.WriteLine($"AES Block Size: {aes.BlockSize}");
        Console.WriteLine($"Key: {Convert.ToBase64String(aes.Key)}");
        Console.WriteLine($"IV: {Convert.ToBase64String(iv)}");

        using MemoryStream ms = new MemoryStream(bytes, iv.Length, bytes.Length - iv.Length);
        using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);
        var res = sr.ReadToEnd();
        Console.WriteLine($"Decrypted: {res}");
        return res;
    }
}