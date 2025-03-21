using Microsoft.JSInterop;

namespace STRING_OBSFUCATE;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class Request
{
    public string value { get; set; }
    public string start { get; set; }
    public string end { get; set; }

    public string type { get; set; }
}