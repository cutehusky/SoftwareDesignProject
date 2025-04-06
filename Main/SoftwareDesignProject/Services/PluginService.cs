using System.Reflection;
using BackendPluginTemplate;
using CommonDTO;
using SoftwareDesignProject.Repositories;
using SoftwareDesignProject.Services.ServerPluginManagement;


namespace SoftwareDesignProject.Services;

public class PluginService: IPluginService
{
    private readonly IPluginRepository _pluginRepository;
    private readonly DynamicPluginManager _pluginManager;
    private readonly string _clientUploadPath;
    private readonly string _serverUploadPath;
    
    public PluginService(IPluginRepository pluginRepository, DynamicPluginManager pluginManager, IHostEnvironment env)
    {
        _pluginRepository = pluginRepository;
        _pluginManager = pluginManager;
        _clientUploadPath = Path.Combine(env.ContentRootPath, "Root/ClientPlugins");
        _serverUploadPath = Path.Combine(env.ContentRootPath, "Root/BackendPlugins");
    }
    
    public async Task<List<PluginDTO>> GetList()
    {
        return await _pluginRepository.GetAll();
    }
    
    public async Task<List<PluginDTO>> GetActiveList()
    {
        return await _pluginRepository.GetActiveList();
    }

    public async Task Rollback()
    {
        
    }

    public async Task AddPlugin(
        string name, 
        string description, 
        string category,
        bool isPremium,
        IFormFile clientDLL, 
        IFormFile? serverDLL)
    {
        
        var clientUid = await GetClientPluginUid(clientDLL);
        if (clientUid == null)
        {
            Console.WriteLine("Fail to load Client DLL");
            throw new InvalidDataException("Fail to load Client DLL");
        }

        Guid? serverUid = null;
        if (serverDLL != null)
        {
            serverUid = await GetServerPluginUid(serverDLL);
            if (serverUid == null)
            {
                Console.WriteLine("Fail to load Server DLL");
                throw new InvalidDataException("Fail to load Server DLL");
            }

            if (serverUid != clientUid)
            {
                Console.WriteLine("Server and client dll must have same id");
                throw new InvalidDataException("Server and client dll must have same id");
            }
        }
        
        var res = await _pluginRepository.Add(new PluginDTO()
        {
            PluginId = (Guid)clientUid,
            Name = name,
            Description = description,
            IsPremium = isPremium,
            Category = category
        });

        if (!res)
        {
            Console.WriteLine("Fail to insert to database");
            throw new InvalidOperationException("Fail to insert to database");
        }

        if (!await SaveFileAsync(clientDLL, clientUid + ".dll", _clientUploadPath))
        {
            await Rollback();
            Console.WriteLine("Fail to save Client Plugin");
            throw new IOException("Fail to save Client Plugin");
        }

        if (serverDLL != null)
        {
            if (!await SaveFileAsync(serverDLL, serverUid + ".dll", _serverUploadPath))
            {
                await Rollback();
                Console.WriteLine("Fail to save Client Plugin");
                throw new IOException("Fail to save Client Plugin");
            }
            await _pluginManager.LoadAllAssemblies();
        }
    }

    public async Task EditPlugin(PluginDTO dto)
    {
        var res = await _pluginRepository.Update(dto);
        if (!res)
        {
            throw new InvalidOperationException("Fail to update in database");
        }
        
        if (dto.IsEnabled != null 
            && (bool)dto.IsEnabled
            && !_pluginManager.CheckLoadedPlugin(dto.PluginId.ToString()))
        {
            await _pluginManager.LoadAllAssemblies();
            return;
        }
        
        if (dto.IsEnabled != null 
            && !(bool)dto.IsEnabled
            && _pluginManager.CheckLoadedPlugin(dto.PluginId.ToString()))
        {
            await _pluginManager.LoadAllAssemblies();
            return;
        }
    }

    public async Task RemovePlugin(Guid id)
    {
        var res = await _pluginRepository.Remove(id);
        if (!res)
        {
            Console.WriteLine("Fail to delete from database");
            throw new InvalidOperationException("Fail to delete from database");
        }

        await RemoveFile(id + ".dll", _clientUploadPath);
        await RemoveFile(id + ".dll", _serverUploadPath);
        if (_pluginManager.CheckLoadedPlugin(id.ToString()))
            await _pluginManager.LoadAllAssemblies();
    }

    public Task<PluginDTO?> GetPluginById(Guid id)
    {
        return _pluginRepository.GetById(id);
    }

    private static async Task<Guid?> GetClientPluginUid(IFormFile file)
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

    private static async Task<Guid?> GetServerPluginUid(IFormFile file)
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
    
    private static async Task<byte[]> SaveByteArrayAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    private static async Task<bool> RemoveFile(string fileName, string path)
    {
        var filePath = Path.Combine(path, fileName);
        if (!File.Exists(filePath))
            return true;
        try
        {
            File.Delete(filePath);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    private static async Task<bool> SaveFileAsync(IFormFile file, string fileName, string uploadPath)
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