using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client;

public interface INavMenuLoader
{
    public Task<List<NavItem>?> GetNavItem();
    public Task<HomeData?> GetHomeItem();
}