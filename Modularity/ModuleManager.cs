using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;

namespace Modularity
{
    class ModuleManager
    {
        private Dictionary<string, ModuleAdapter> modules = new Dictionary<string, ModuleAdapter>();
        private EventSystem EventSystem = new EventSystem();

        public ModuleManager(EventSystem EventSystem)
        {
            this.EventSystem = EventSystem;
            this.SetupModule(new ErrorHandler(this));
        }


        public void SetupModule(ModuleAdapter module)
        {
            if(module == null)
                this.EventSystem.RaiseEvent("module_manager", "error", "Error while installing a null module");

            if (this.modules.ContainsKey(module.name))
            {
                this.EventSystem.RaiseEvent("module_manager", "error", string.Format("Impossible to install the module {0} because an other module with the same name is already install", module.name));
                return;
            }

            bool valid = true;
            string missing_module_name = null;
            if (module.dependancy != null)
            {
                foreach (Dependancy m in module.dependancy)
                {
                    missing_module_name = m.name;
                    valid = valid && this.CheckModule(m);
                }
            }

            if (valid)
            {
                module.Initialize();
                this.EventSystem.RaiseEvent("module_manager", "installed", module.name);
                this.modules.Add(module.name, module);
            }
            else
            {
                this.EventSystem.RaiseEvent("module_manager", "error", string.Format("The module \"{0}\" needs the module \"{1}\" to be installed and with a compatible version", module.name, missing_module_name));
            }
        }

        public void UninstallModule(ModuleAdapter module)
        {
            foreach (Watcher watcher in module.watchers)
            {
                this.EventSystem.DelWatcher(watcher);
            }

            this.modules.Remove(module.name);
        }

        public void RequestWatcher(ModuleAdapter requester, string event_identifier, Effect effect)
        {   
            requester.watchers.Add(EventSystem.AddWatcher("*", event_identifier, effect, x => true));
        }

        public void RequestWatcher(ModuleAdapter requester, string event_emitter, string event_identifier, Effect effect)
        {
            requester.watchers.Add(EventSystem.AddWatcher(event_emitter, event_identifier, effect, x => true));
        }

        public void RaiseEvent(ModuleAdapter emitter, string event_identifier, params object[] o)
        {
            this.EventSystem.RaiseEvent(emitter.name, event_identifier, o);
        }

        private bool CheckModule(Dependancy dependancy)
        {
            return this.modules.ContainsKey(dependancy.name) && dependancy.version_predicate(this.modules[dependancy.name].version);
        }
    }
}
