using Microsoft.AspNetCore.Mvc.Routing;

namespace SoftwareDesignProject.Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

public class DynamicRouteTransformer : DynamicRouteValueTransformer
{
    private readonly ApplicationPartManager _partManager;

    public DynamicRouteTransformer(ApplicationPartManager partManager)
    {
        _partManager = partManager;
    }

    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        string? controllerName = values["controller"]?.ToString();
        string? actionName = values["action"]?.ToString();
        
        Console.WriteLine("Controller name: " + controllerName);
        Console.WriteLine("Action name: " + actionName);

        if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            return new ValueTask<RouteValueDictionary>();

        // Check if the controller exists in the loaded assemblies
        bool controllerExists = _partManager.ApplicationParts
            .OfType<AssemblyPart>()
            .SelectMany(ap => ap.Assembly.GetTypes())
            .Any(t => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(t) && 
                      t.Name.Equals(controllerName + "Controller", StringComparison.OrdinalIgnoreCase));
        
        if (!controllerExists)
            return new ValueTask<RouteValueDictionary>();
        Console.WriteLine(values);
        return new ValueTask<RouteValueDictionary>(values);
    }
}
