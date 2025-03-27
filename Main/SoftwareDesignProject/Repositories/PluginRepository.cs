using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Repositories;

public class PluginRepository
{
    private readonly AppDbContext _dbContext;

    public PluginRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
}