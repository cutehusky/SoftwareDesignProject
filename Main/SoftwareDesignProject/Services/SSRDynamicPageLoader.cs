using SoftwareDesignProject.Client;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Services;

// empty service to prevent exception when force refresh page as SSR
public class SSRDynamicPageLoader: IDynamicPageLoader
{
    public Task<DynamicPage?> LoadDynamicAssembly(string dllUrl)
    {
        return Task.FromResult((DynamicPage?) null);
    }
}