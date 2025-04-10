using System.Reflection;
using BackendPluginTemplate;
using CommonDTO;
using MudBlazor;
using SoftwareDesignProject.Models.DTOMapper;
using SoftwareDesignProject.Repositories;
using SoftwareDesignProject.Services.ServerPluginManagement;


namespace SoftwareDesignProject.Services;

public class PluginService : IPluginService
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

    public async Task<PaginationList<PluginDTO>> GetList(int page, int pageSize, string sortBy,
        SortDirection order, string search, UserRoles? userRole)
    {
        var query = await _pluginRepository.GetAll(page, pageSize, sortBy, order, search);

        if (userRole < UserRoles.Premium)
        {
            query.Items = query.Items.Where(p => p.IsPremium == false).ToList();
        }

        return query;
    }

    public async Task<List<PluginDTO>> GetActiveList(UserRoles? userRole)
    {
        var query = await _pluginRepository.GetActiveList();

        if (userRole < UserRoles.Premium)
        {
            query = query.Where(p => p.IsPremium == false).ToList();
        }
        return query;
    }

    private readonly Stack<Func<Task>> _rollbackActions = new();

    private async Task Rollback()
    {
        while (_rollbackActions.Count > 0)
        {
            var action = _rollbackActions.Pop();
            try
            {
                await action();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
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

        _rollbackActions.Push(async () =>
        {
            if (!await _pluginRepository.Remove((Guid)clientUid))
            {
                Console.WriteLine("Fail to delete from database");
            }
        });

        if (!await SaveFileAsync(clientDLL, clientUid + ".dll", _clientUploadPath))
        {
            await Rollback();
            Console.WriteLine("Fail to save Client Plugin");
            throw new IOException("Fail to save Client Plugin");
        }

        _rollbackActions.Push(async () =>
        {
            if (!await RemoveFile(clientUid + ".dll", _clientUploadPath))
            {
                Console.WriteLine("Fail to delete Client Plugin");
            }
        });

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
        Console.WriteLine("Plugin added successfully");
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
        Console.WriteLine("Plugin edited successfully");
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
        Console.WriteLine("Plugin removed successfully");
    }

    public Task<PluginDTO?> GetPluginById(Guid id)
    {
        return _pluginRepository.GetById(id);
    }

    public async Task UpgradePlugin(Guid pluginId,
        IFormFile? clientDll, IFormFile? serverDll)
    {
        Guid? clientUid = null;
        if (clientDll != null)
        {
            clientUid = GetClientPluginUid(clientDll).Result;
            if (clientUid == null)
            {
                Console.WriteLine("Fail to load Client DLL");
                throw new InvalidDataException("Fail to load Client DLL");
            }

            if (pluginId != clientUid)
            {
                Console.WriteLine("Plugin ID and Client DLL ID must be same");
                throw new InvalidDataException("Plugin ID and Client DLL ID must be same");
            }
        }

        Guid? serverUid = null;
        if (serverDll != null)
        {
            serverUid = GetServerPluginUid(serverDll).Result;
            if (serverUid == null)
            {
                Console.WriteLine("Fail to load Server DLL");
                throw new InvalidDataException("Fail to load Server DLL");
            }

            if (serverUid != pluginId)
            {
                Console.WriteLine("Plugin ID and Server DLL ID must be same");
                throw new InvalidDataException("Plugin ID and Server DLL ID must be same");
            }
        }

        if (clientDll != null)
        {
            await BackupFile(pluginId + ".dll", _clientUploadPath);
            _rollbackActions.Push(async () =>
            {
                if (!await RemoveFile(pluginId + ".dll", _clientUploadPath))
                {
                    Console.WriteLine("Fail to delete Client Plugin");
                }
                if (!await RestoreFile(pluginId + ".dll", _clientUploadPath))
                {
                    Console.WriteLine("Fail to restore Client Plugin");
                }
            });

            if (!await SaveFileAsync(clientDll, clientUid + ".dll", _clientUploadPath))
            {
                await Rollback();
                Console.WriteLine("Fail to save Client Plugin");
                throw new IOException("Fail to save Client Plugin");
            }
        }

        if (serverDll != null)
        {
            await BackupFile(pluginId + ".dll", _serverUploadPath);
            _rollbackActions.Push(async () =>
            {
                if (!await RemoveFile(pluginId + ".dll", _serverUploadPath))
                {
                    Console.WriteLine("Fail to delete Server Plugin");
                }
                if (!await RestoreFile(pluginId + ".dll", _serverUploadPath))
                {
                    Console.WriteLine("Fail to restore Server Plugin");
                }
                await _pluginManager.LoadAllAssemblies();
            });

            if (!await SaveFileAsync(serverDll, serverUid + ".dll", _serverUploadPath))
            {
                await Rollback();
                Console.WriteLine("Fail to save Client Plugin");
                throw new IOException("Fail to save Client Plugin");
            }
            await _pluginManager.LoadAllAssemblies();
        }

        await RemoveBackup(pluginId + ".dll", _clientUploadPath);
        await RemoveBackup(pluginId + ".dll", _serverUploadPath);
        Console.WriteLine("Plugin upgraded successfully");
    }

    private static async Task<bool> RemoveBackup(string fileName, string path)
    {
        var backupPath = Path.Combine(path, "backup", fileName);
        if (!File.Exists(backupPath))
            return false;
        try
        {
            File.Delete(backupPath);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    private static async Task<bool> RestoreFile(string fileName, string path)
    {
        var filePath = Path.Combine(path, fileName);
        var backupPath = Path.Combine(path, "backup", fileName);
        if (!File.Exists(backupPath))
            return false;
        try
        {
            File.Copy(backupPath, filePath, true);
            File.Delete(backupPath);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    private static async Task<bool> BackupFile(string fileName, string path)
    {
        var filePath = Path.Combine(path, fileName);
        var backupPath = Path.Combine(path, "backup", fileName);
        if (!Directory.Exists(Path.Combine(path, "backup")))
            Directory.CreateDirectory(Path.Combine(path, "backup"));
        if (!File.Exists(filePath))
            return false;
        try
        {
            File.Copy(filePath, backupPath, true);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
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