using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SoftwareDesignProject.Services;

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

public class DynamicPluginManager
{
    private readonly ApplicationPartManager _partManager;
    private readonly string _pluginPath;
    private readonly ActionDescriptorChangeProvider _changeProvider;
    private readonly HashSet<string> _loadedPlugin = new();
    private readonly Dictionary<string, List<Type>> _controllerCollection = new();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    private IServiceProvider _serviceProvider;

    public DynamicPluginManager(ApplicationPartManager partManager, 
        IHostEnvironment env, IActionDescriptorChangeProvider changeProvider)
    {
        _partManager = partManager;
        _changeProvider = (changeProvider as ActionDescriptorChangeProvider)!;
        _pluginPath = Path.Combine(env.ContentRootPath, "Root/BackendPlugins");
        if (!Directory.Exists(_pluginPath))
            Directory.CreateDirectory(_pluginPath);
        LoadAllAssemblies();
    }
    
    public void LoadAllAssemblies()
    {
        while (_partManager.ApplicationParts.Count > 1)
            _partManager.ApplicationParts.RemoveAt(1);
        _controllerCollection.Clear();
        _serviceCollection.Clear();
        _loadedPlugin.Clear();
        foreach (var dll in Directory.GetFiles(_pluginPath, "*.dll"))
        {
            LoadAssembly(dll);
        }
        _changeProvider.NotifyChanges();
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }

    private void LoadAssembly(string path)
    {
        var dllBytes = File.ReadAllBytes(path);
        var assembly = Assembly.Load(dllBytes);
        var pluginId = "";
        foreach (var type in assembly.GetTypes())
        {
            if (!(typeof(IConfig)).IsAssignableFrom(type) || !type.IsClass) 
                continue;
            IConfig config = (Activator.CreateInstance(type) as IConfig)!;
            pluginId = config.ID;
            _controllerCollection.TryAdd(pluginId, config.ExportedControllers);
            try
            {
                config.RegisterService(_serviceCollection);
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
        Console.WriteLine("Loaded assembly: {0}", assembly.FullName);
    }

    public bool CheckLoadedPlugin(string uid)
    {
        return _loadedPlugin.Contains(uid);
    }

    public IServiceProvider GetServiceProvider()
    {
        return _serviceProvider;
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
}
