using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;

namespace Modularity
{
    abstract class ModuleAdapter
    {
        public string name { get; private set; }
        public float version { get; private set; }
        public Dependancy[] dependancy { get; private set; }
        protected ModuleManager manager { get; private set; }
        public List<Watcher> watchers { get; set; }

        public virtual void Initialize() { }

        public ModuleAdapter(string name, float version, ModuleManager manager)
        {
            this.name = name;
            this.version = version;
            this.manager = manager;
            this.watchers = new List<Watcher>();
        }

        public ModuleAdapter(string name, float version, Dependancy[] dependancy, ModuleManager manager)
        {
            this.name = name;
            this.version = version;
            this.dependancy = dependancy;
            this.manager = manager;
            this.watchers = new List<Watcher>();
        }
    }
}
