using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;

namespace Modularity
{
    class ErrorHandler : ModuleAdapter
    {
        public ErrorHandler(ModuleManager manager) : base("error_handler", 1.0f, manager)
        {
        }

        public override void Initialize()
        {
            this.manager.RequestWatcher(this, "error", Effect.Create<string>(this.HandleError));
            this.manager.RequestWatcher(this, "warning", Effect.Create<string>(this.HandleWarning));
        }

        private void HandleError(string error)
        {
            Console.WriteLine("<Error> " + error);
            Console.WriteLine("Press Enter to exit");
            Console.Read();
            Environment.Exit(0);

        }

        private void HandleWarning(string warning)
        {
            Console.WriteLine("<Warning> " + warning);
        }
    }
}
