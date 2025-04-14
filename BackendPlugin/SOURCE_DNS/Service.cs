using System.Net;
using System.Net.Sockets;

namespace SOURCE_DNS
{
    public class Service
    {
        public async Task<List<string>> ResolveDns(string value)
        {
            string host;

            // Try to parse value as a URI.
            if (Uri.TryCreate(value, UriKind.Absolute, out var uriResult))
            {
                host = uriResult.Host;
            }
            else
            {
                host = value;
            }

            try
            {
                var addresses = await Dns.GetHostAddressesAsync(host);
                return addresses.Select(ip => ip.ToString()).ToList();

            }
            catch (SocketException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error resolving DNS for {host}: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                // Handle other exceptions as needed
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<string>();
            }
        }
    }
}
