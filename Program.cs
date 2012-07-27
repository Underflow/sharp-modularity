using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;
using Modularity;
using Scripts;

namespace Modularite
{
    class Program : ModuleAdapter
    {
        private static ScriptEngine scriptEngine = new LUAScriptEngine();

        static void ModuleInstalled(string name)
        {
            Console.WriteLine("Module \"{0}\" successfully installed", name);
        }

        static void Main(string[] args)
        {
            Program prgrm = new Program();
            prgrm.Run();
        }

        public Program()
            : base("program", 1.0f, new ModuleManager(new EventSystem()))
        {
            this.manager.SetupModule(this);
        }

        public override void Initialize()
        {
            this.manager.RequestWatcher(this, "module_manager", "installed", Effect.Create<string>(ModuleInstalled));

            this.manager.SetupModule(new SayHello(this.manager));
        }

        public void Run()
        {
            this.manager.RaiseEvent(this, "initialized");
            Console.Read();
        }
    }
}
