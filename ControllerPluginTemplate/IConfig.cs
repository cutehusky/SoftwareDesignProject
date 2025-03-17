using System;
using System.Collections.Generic;

namespace ControllerPluginTemplate;

public interface IConfig
{
    public string ID { get; }
    public List<Type> ExportedControllers { get; } 
}