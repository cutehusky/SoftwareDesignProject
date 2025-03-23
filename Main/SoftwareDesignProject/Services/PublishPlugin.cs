using SoftwareDesignProject.Client.Models;
using SoftwareDesignProject.Client.Services;

namespace SoftwareDesignProject.Services;

public class PublishPlugin: IPublishPlugin
{
    public Task<bool> Submit(PluginUploadData data)
    {
        return Task.FromResult<bool>(false);
    }
}