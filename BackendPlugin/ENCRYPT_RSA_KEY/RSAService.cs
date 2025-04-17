using System.Security.Cryptography;

namespace ENCRYPT_RSA_KEY;

public class RSAService
{
    public Tuple<string, string> GenerateRSAKeyXML(int requestKeySize)
    {
        using var rsa = new RSACryptoServiceProvider(requestKeySize);
        var publicKey = rsa.ToXmlString(false);
        var privateKey = rsa.ToXmlString(true);
        return Tuple.Create(publicKey, privateKey);
    }
    
    public Tuple<string, string> GenerateRSAKeyPEM(int requestKeySize)
    {
        using var rsa = new RSACryptoServiceProvider(requestKeySize);
        return Tuple.Create(rsa.ExportRSAPublicKeyPem(), rsa.ExportRSAPrivateKeyPem());
    }
}