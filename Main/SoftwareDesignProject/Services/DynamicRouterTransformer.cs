using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SoftwareDesignProject.Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class DynamicRouteTransformer : DynamicRouteValueTransformer
{
    private readonly DynamicPluginManager _pluginPluginManager;

    public DynamicRouteTransformer(DynamicPluginManager pluginPluginManager)
    {
        _pluginPluginManager = pluginPluginManager;
    }

    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        string? controllerName = values["controller"]?.ToString();
        string? actionName = values["action"]?.ToString();
        string? id = values["id"]?.ToString();
        
        Console.WriteLine("Controller name: " + controllerName);
        Console.WriteLine("Action name: " + actionName);
        Console.WriteLine("ID: " + id);

        if (string.IsNullOrEmpty(controllerName) 
            || string.IsNullOrEmpty(actionName) 
            || string.IsNullOrEmpty(id))
            return new ValueTask<RouteValueDictionary>(); 

        // Check if the controller exists in the loaded assemblies
        var controller = _pluginPluginManager.GetValidControllers(id, controllerName);
        
        if (controller.Count == 0)
            return new ValueTask<RouteValueDictionary>();
        
        if (controller.Count != 1)
            throw new AmbiguousActionException("More than 1 controller matched");
        
        values["namespace"] = controller[0].Namespace;
        Console.WriteLine("Using controller: " + controller[0].FullName);
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
