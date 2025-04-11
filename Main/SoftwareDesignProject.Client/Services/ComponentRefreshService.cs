namespace SoftwareDesignProject.Client.Services;

public class ComponentRefreshService: IComponentRefreshService
{
    public event IComponentRefreshService.OnRefresh? Refresh;
    public void NotifyStateChanged(object sender, EventArgs? e = null)
    {
        Refresh?.Invoke(sender, e);
    }
}