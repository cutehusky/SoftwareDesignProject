namespace SoftwareDesignProject.Client;

public interface IDynamicPageLoader
{
    public Task<DynamicPage?> LoadDynamicAssembly(string dllUrl);
}