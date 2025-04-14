using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SoftwareDesignProject.Services.ServerPluginManagement;

public class DynamicRouteTransformer : DynamicRouteValueTransformer
{
    private readonly DynamicPluginManager _pluginPluginManager;
    private ILogger<DynamicRouteTransformer> _logger;

    public DynamicRouteTransformer(DynamicPluginManager pluginPluginManager, ILogger<DynamicRouteTransformer> logger)
    {
        _pluginPluginManager = pluginPluginManager;
        _logger = logger;
    }

    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        string? controllerName = values["controller"]?.ToString();
        string? actionName = values["action"]?.ToString();
        string? id = values["id"]?.ToString();
        
        _logger.LogTrace($"Plugin {id} API using controller: " + controllerName + " action: " + actionName);

        if (string.IsNullOrEmpty(controllerName) 
            || string.IsNullOrEmpty(actionName) 
            || string.IsNullOrEmpty(id))
            return new ValueTask<RouteValueDictionary>(); 

        // TODO: check if plugin is premium
        // Check if the controller exists in the loaded assemblies
        var controller = _pluginPluginManager.GetValidControllers(id, controllerName);
        
        if (controller.Count == 0)
            return new ValueTask<RouteValueDictionary>();
        
        if (controller.Count != 1)
            throw new AmbiguousActionException("More than 1 controller matched");
        
        values["namespace"] = controller[0].Namespace;
        _logger.LogTrace($"Plugin {id} API using controller: " + controller[0].FullName);
        return new ValueTask<RouteValueDictionary>(values);
    }

    public override ValueTask<IReadOnlyList<Endpoint>> FilterAsync(HttpContext httpContext, RouteValueDictionary values, IReadOnlyList<Endpoint> endpoints)
    {
        if (!values.TryGetValue("namespace", out var res) 
            || res is not string controllerNamespace)
            return base.FilterAsync(httpContext, values, endpoints);
        
        var finalEndpoint = new List<Endpoint>();
        foreach (var endpoint in endpoints)
        {
            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (actionDescriptor == null) 
                continue;
            Console.WriteLine(actionDescriptor.ControllerTypeInfo.Namespace);
            if (actionDescriptor.ControllerTypeInfo.Namespace != controllerNamespace) 
                continue;
            finalEndpoint.Add(endpoint);
            break;
        }
        return new ValueTask<IReadOnlyList<Endpoint>>(finalEndpoint);
    }
}
