using System.Reflection;
using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SoftwareDesignProject.Repositories;

namespace SoftwareDesignProject.Services.ServerPluginManagement;

public class DynamicPluginManager: IHostedService
{
    private readonly ApplicationPartManager _partManager;
    private readonly string _pluginPath;
    private readonly ActionDescriptorChangeProvider _changeProvider;
    private readonly HashSet<string> _loadedPlugin = new();
    private readonly Dictionary<string, List<Type>> _controllerCollection = new();
    private readonly IServiceCollection _dynamicServiceCollection = new ServiceCollection();
    private readonly IServiceProvider _serviceProvider;
    private IServiceProvider _dynamicServiceProvider = null!;
    private readonly ILogger<DynamicPluginManager> _logger;

    public DynamicPluginManager(
        ApplicationPartManager partManager,
        IServiceProvider serviceProvider,
        IHostEnvironment env, 
        IActionDescriptorChangeProvider changeProvider, 
        ILogger<DynamicPluginManager> logger)
    {
        _partManager = partManager;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _changeProvider = (changeProvider as ActionDescriptorChangeProvider)!;
        _pluginPath = Path.Combine(env.ContentRootPath, "Root/BackendPlugins");
        if (!Directory.Exists(_pluginPath))
            Directory.CreateDirectory(_pluginPath);
    }
    
    public async Task LoadAllAssemblies()
    {
        while (_partManager.ApplicationParts.Count > 1)
            _partManager.ApplicationParts.RemoveAt(1);
        _controllerCollection.Clear();
        _dynamicServiceCollection.Clear();
        _loadedPlugin.Clear();
        using var scope = _serviceProvider.CreateScope();
        var pluginRepository = scope.ServiceProvider.GetRequiredService<IPluginRepository>();
        foreach (var dll in Directory.GetFiles(_pluginPath, "*.dll"))
        {
            await LoadAssembly(pluginRepository, dll);
        }
        _changeProvider.NotifyChanges();
        _dynamicServiceProvider = _dynamicServiceCollection.BuildServiceProvider();
    }

    private async Task LoadAssembly(
        IPluginRepository pluginRepository,
        string path)
    {
        _logger.LogInformation("Loading assembly: {0}", path);
        var dllBytes = await File.ReadAllBytesAsync(path);
        var assembly = Assembly.Load(dllBytes);
        var pluginId = "";
        foreach (var type in assembly.GetTypes())
        {
            if (!(typeof(IConfig)).IsAssignableFrom(type) || !type.IsClass) 
                continue;
            IConfig config = (Activator.CreateInstance(type) as IConfig)!;
            pluginId = config.ID;
            
            if (!Guid.TryParse(pluginId, out var result))
            {
                _logger.LogError("Invalid plugin ID: {0}", pluginId);
                return;
            }
            
            if (CheckLoadedPlugin(pluginId))
            {
                _logger.LogWarning("Plugin already loaded: {0}", pluginId);
                return;
            }
            
            var plugin = await pluginRepository.GetById(result);
            if (plugin == null)
            {
                _logger.LogWarning("Plugin not registered in database: {0}", pluginId);
                return;
            }
            
            if (!(bool)plugin.IsEnabled!)
            {
                _logger.LogWarning("Plugin is disabled: {0}", pluginId);
                return;
            }
            
            _controllerCollection.TryAdd(pluginId, config.ExportedControllers);
            try
            {
                config.RegisterService(_dynamicServiceCollection);
            }
            catch (Exception e)
            {
                // ignored
            }
            break;
        }

        if (pluginId.Length == 0)
            return;
        _partManager.ApplicationParts.Add(new AssemblyPart(assembly));
        _loadedPlugin.Add(pluginId);
        _logger.LogInformation("Loaded assembly: {0}", assembly.FullName);
    }

    public bool CheckLoadedPlugin(string uid)
    {
        return _loadedPlugin.Contains(uid);
    }

    public IServiceProvider GetServiceProvider()
    {
        return _dynamicServiceProvider;
    }
    
    public List<Type> GetControllers(string pluginId)
    {
        return _controllerCollection.TryGetValue(pluginId, out var res) ? res : [];
    }
    
    public List<Type> GetValidControllers(string pluginId, string controllerName)
    {
        var controller = GetControllers(pluginId)
            .Where(t => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(t) && 
                        t.Name.Equals(controllerName + "Controller", StringComparison.OrdinalIgnoreCase))
            .ToList();
        return controller;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await LoadAllAssemblies();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
