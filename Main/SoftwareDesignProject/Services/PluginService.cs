using System.Reflection;
using BackendPluginTemplate;
using CommonDTO;
using SoftwareDesignProject.Models.Entities;
using SoftwareDesignProject.Repositories;


namespace SoftwareDesignProject.Services;

public class PluginService
{
    private PluginRepository _pluginRepository;
    private readonly IHostEnvironment _env;
    private readonly string _clientUploadPath;
    private readonly string _serverUploadPath;
    
    public PluginService(PluginRepository pluginRepository, IHostEnvironment env)
    {
        _pluginRepository = pluginRepository;
        _env = env;
        
        _clientUploadPath = Path.Combine(_env.ContentRootPath, "Root/ClientPlugins");
        _serverUploadPath = Path.Combine(_env.ContentRootPath, "Root/BackendPlugins");
    }
    
    public async Task<List<PluginDTO>> GetList()
    {
        return await _pluginRepository.GetAll();
    }

    public async Task Rollback()
    {
        
    }

    public async Task<bool> Add(PluginDTO pluginDto)
    {
        if (string.IsNullOrEmpty(pluginDto.Category))
            pluginDto.Category = Plugin.DefaultCategory;
        if (string.IsNullOrEmpty(pluginDto.Name))
            pluginDto.Name = pluginDto.PluginId + " Plugin";
        if (string.IsNullOrEmpty(pluginDto.Description))
            pluginDto.Description = "This is plugin with name: " + pluginDto.Name;
        var res = await _pluginRepository.Add(pluginDto);
        if (res)
            return true;
        await Rollback();
        return false;
    }

    private async Task<Guid?> GetClientPluginUid(IFormFile file)
    {
        var dllBytes = await SaveByteArrayAsync(file);
        var assembly = Assembly.Load(dllBytes);

        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsClass || !typeof(ClientPluginTemplate.IConfig).IsAssignableFrom(type)) 
                continue;
            var config = (Activator.CreateInstance(type) as ClientPluginTemplate.IConfig)!;
            if (Guid.TryParse(config.ID, out var result))
            {
                return result;
            }
        }
        return null;
    }

    public async Task<Guid?> SaveClientDLL(IFormFile file)
    {
        var uid = await GetClientPluginUid(file);
        if (uid == null)
        {
            await Rollback();
            return null;
        }
        if (await SaveFileAsync(file, uid + ".dll", _clientUploadPath)) 
            return uid;
        await Rollback();
        return null;
    }

    private async Task<Guid?> GetServerPluginUid(IFormFile file)
    {
        var dllBytes = await SaveByteArrayAsync(file);
        var assembly = Assembly.Load(dllBytes);
        
        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsClass || !typeof(IConfig).IsAssignableFrom(type)) 
                continue;
            var config = (Activator.CreateInstance(type) as IConfig)!;
            if (!Guid.TryParse(config.ID, out var result)) 
                continue;
            return result;
        }
        return null;
    }
    
    public async Task<Guid?> SaveServerDLL(IFormFile file)
    {
        var uid = await GetServerPluginUid(file);
        if (uid == null)
        {
            await Rollback();
            return null;
        }
        if (await SaveFileAsync(file, uid + ".dll", _serverUploadPath)) 
            return uid;
        await Rollback();
        return null;
    }
    
    private static async Task<byte[]> SaveByteArrayAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
    
    private async Task<bool> SaveFileAsync(IFormFile file, string fileName, string uploadPath)
    {
        Console.WriteLine(file.Name);
        Console.WriteLine(file.Length);
        
        var filePath = Path.Combine(uploadPath, fileName);
        try
        {
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
}