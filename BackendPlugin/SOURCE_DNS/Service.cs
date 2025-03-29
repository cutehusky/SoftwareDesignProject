using System.Net;

namespace SOURCE_DNS
{
    public class Service
    {
        public async Task<List<string>> ResolveDns(string value)
        {
            var addresses = await Dns.GetHostAddressesAsync(value);
            return addresses.Select(ip => ip.ToString()).ToList();
        }
    }
}
