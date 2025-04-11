using CommonDTO;

namespace SoftwareDesignProject.Client.Services;

public interface INavMenuLoader
{
    public Task<NavData?> GetNavItem();
    public Task<HomeData?> GetHomeItem();
    public Task<List<NavItem>?> Search(string queryValue);
}