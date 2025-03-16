namespace SoftwareDesignProject.Services;

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

public class DynamicControllerLoader
{
    private readonly ApplicationPartManager _partManager;
    private readonly IHostEnvironment _env;
    private readonly string _pluginPath;
    private readonly ActionDescriptorChangeProvider _changeProvider;

    public DynamicControllerLoader(ApplicationPartManager partManager, 
        IHostEnvironment env, ActionDescriptorChangeProvider changeProvider)
    {
        _partManager = partManager;
        _env = env;
        _changeProvider = changeProvider;
        _pluginPath = Path.Combine(_env.ContentRootPath, "Root/controllerPlugins");
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
        foreach (var dll in Directory.GetFiles(_pluginPath, "*.dll"))
        {
            LoadAssembly(dll);
        }
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

    private void LoadAssembly(string path)
    {
        var assembly = Assembly.LoadFrom(path);
        _partManager.ApplicationParts.Add(new AssemblyPart(assembly));
        _changeProvider.NotifyChanges();
        Console.WriteLine("Loaded assembly: {0}", assembly.FullName);
    }
}
