namespace HASH_PASSWORD;

public class HashResponse
{
    public string hashed { get; set; }
}

public class VerifyResponse
{
    public bool ok { get; set; }
}