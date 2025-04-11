namespace SoftwareDesignProject.Client.Services;

public interface IComponentRefreshService
{
    delegate void OnRefresh(object sender, EventArgs? e);
    event OnRefresh? Refresh;
    void NotifyStateChanged(object sender, EventArgs? e = null);
}