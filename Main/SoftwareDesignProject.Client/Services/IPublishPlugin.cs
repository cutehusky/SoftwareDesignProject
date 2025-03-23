using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public interface IPublishPlugin
{
    public Task<bool> Submit(PluginUploadData data);
}