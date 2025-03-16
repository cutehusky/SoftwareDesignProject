using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTemplate
{
    public interface IConfig
    {
        public string ID { get; }
        public Type EntryPoint { get; }
        public string APIEndPoint { set; }
    }
}
