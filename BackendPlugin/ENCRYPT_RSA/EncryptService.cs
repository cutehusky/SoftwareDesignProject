using System.Security.Cryptography;
using System.Text;

namespace ENCRYPT_RSA;

public class EncryptService
{
    public string Encrypt_XML(string plainText, string publicKey)
    {
        using RSA rsa = RSA.Create();
        rsa.FromXmlString(publicKey);
        var dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
        var res = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(res);
    }

    public string Decrypt_XML(string cipherText, string privateKey)
    {
        byte[] dataToDecrypt = Convert.FromBase64String(cipherText);
        using RSA rsa = RSA.Create();
        rsa.FromXmlString(privateKey);
        var decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedData);
    }
    
    public string Encrypt_PEM(string plainText, string publicKey)
    {
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(publicKey);
        var dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
        var res = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(res);
    }

    public string Decrypt_PEM(string cipherText, string privateKey)
    {
        byte[] dataToDecrypt = Convert.FromBase64String(cipherText);
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);
        var decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedData);
    }
}