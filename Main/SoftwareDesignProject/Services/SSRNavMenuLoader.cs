using CommonDTO;
using SoftwareDesignProject.Client;

namespace SoftwareDesignProject.Services;

public class SSRNavMenuLoader: INavMenuLoader
{
    public Task<List<NavItem>?> GetNavItem()
    {
        return Task.FromResult((List<NavItem>?) null);
    }
}