namespace HASH_PASSWORD;

public class HashRequest
{
    public string value { get; set; }
    public int salt { get; set; }
}

public class VerifyRequest
{
    public string value { get; set; }
    public string hashed { get; set; }
}