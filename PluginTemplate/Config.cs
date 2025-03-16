using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTemplate
{
    public interface Config
    {
        public string ID { get; }
        public Type EntryPoint { get; }
    }
}
