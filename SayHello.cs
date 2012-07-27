using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;
using Modularity;

namespace Modularite
{
    class SayHello : ModuleAdapter
    {
        public SayHello(ModuleManager manager) 
            : base("say_hello", 1.0f, new Dependancy[] {new Dependancy("error_handler")}, manager)
        {

        }

        public override void Initialize()
        {
            this.manager.RequestWatcher(this, "program", "initialized", Effect.Create(this.AskName));
            this.manager.RequestWatcher(this, "say_hello", "NameAsked", Effect.Create<string>(this.SayHelloTo));
        }

        public void AskName()
        {
            Console.WriteLine("What is your name ?");
            string name = Console.ReadLine();
            if (name != "")
            {
                this.manager.RaiseEvent(this, "NameAsked", name);
            }
            else
            {
                this.manager.RaiseEvent(this, "error", "The name is empty");
            }
        }

        private void SayHelloTo(string name)
        {
            Console.WriteLine("Hello " + name);
        }
    }
}
