using ControllerPluginTemplate;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SoftwareDesignProject.Services;

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

public class DynamicControllerLoader
{
    private readonly ApplicationPartManager _partManager;
    private readonly string _pluginPath;
    private readonly ActionDescriptorChangeProvider _changeProvider;
    private readonly Dictionary<string, List<Type>> _controllerMapping = new();

    public DynamicControllerLoader(ApplicationPartManager partManager, 
        IHostEnvironment env, IActionDescriptorChangeProvider changeProvider)
    {
        _partManager = partManager;
        _changeProvider = (changeProvider as ActionDescriptorChangeProvider)!;
        _pluginPath = Path.Combine(env.ContentRootPath, "Root/controllerPlugins");
        if (!Directory.Exists(_pluginPath))
            Directory.CreateDirectory(_pluginPath);
        LoadAllAssemblies();
        //StartWatching();
    }

    private void StartWatching()
    {
        var watcher = new FileSystemWatcher(_pluginPath, "*.dll");
        watcher.Created += OnNewDllDetected;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Watching for DLL changes in {0}", _pluginPath);
    }
    
    public void LoadAllAssemblies()
    {
        while (_partManager.ApplicationParts.Count > 1)
            _partManager.ApplicationParts.RemoveAt(1);
        _controllerMapping.Clear();
        foreach (var dll in Directory.GetFiles(_pluginPath, "*.dll"))
        {
            LoadAssembly(dll);
        }
        _changeProvider.NotifyChanges();
    }

    private void OnNewDllDetected(object sender, FileSystemEventArgs e)
    {
        try
        {
            Console.WriteLine("Detected new DLL: {0}", e.FullPath);
            LoadAssembly(e.FullPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading DLL: " + ex.Message);
        }
    }

    public List<Type> GetControllers(string pluginId)
    {
        return _controllerMapping.TryGetValue(pluginId, out var res) ? res : [];
    }

    private void LoadAssembly(string path)
    {
        var dllBytes = File.ReadAllBytes(path);
        var assembly = Assembly.Load(dllBytes);
        var pluginId = "";
        var controllers = new List<Type>();
        foreach (var type in assembly.GetTypes())
        {
            if ((typeof(IConfig)).IsAssignableFrom(type) && type.IsClass)
            {
                IConfig config = (Activator.CreateInstance(type) as IConfig)!;
                pluginId = config.ID;
                controllers = config.ExportedControllers;
                break;
            }
        }

        if (controllers.Count == 0 || pluginId.Length == 0)
            return;
        _controllerMapping.TryAdd(pluginId, controllers);
        _partManager.ApplicationParts.Add(new AssemblyPart(assembly));
        Console.WriteLine("Loaded assembly: {0}", assembly.FullName);
    }
}
