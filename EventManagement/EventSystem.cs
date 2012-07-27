using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    class EventSystem
    {
        public bool debug { get; set; }
        private Dictionary<string, List<Watcher>> watchers = new Dictionary<string, List<Watcher>>();


        public Watcher AddWatcher(string event_identifier, Effect effect)
        {
            return this.AddWatcher(event_identifier, effect, x => true);
        }

        public Watcher AddWatcher(string event_emitter, string event_identifier, Effect effect)
        {
            return this.AddWatcher(event_emitter, event_identifier, effect, x => true);
        }
        
        public Watcher AddWatcher(string event_identifier, Effect effect, Watcher.WatcherCondition condition)
        {
            Watcher watcher = new Watcher(event_identifier, effect, condition);
            if (this.watchers.ContainsKey(watcher.event_identifier))
                this.watchers[watcher.event_identifier].Add(watcher);
            else
            {
                this.watchers[watcher.event_identifier] = new List<Watcher>();
                this.watchers[watcher.event_identifier].Add(watcher);
            }
            return watcher;
        }

        public Watcher AddWatcher(string event_emitter, string event_identifier, Effect effect, Watcher.WatcherCondition condition)
        {
            Watcher watcher = new Watcher(event_emitter, event_identifier, effect, condition);
            if (this.watchers.ContainsKey(watcher.event_identifier))
                this.watchers[watcher.event_identifier].Add(watcher);
            else
            {
                this.watchers[watcher.event_identifier] = new List<Watcher>();
                this.watchers[watcher.event_identifier].Add(watcher);
            }
            return watcher;
        }

        public bool DelWatcher(Watcher watcher)
        {
            if (this.watchers.ContainsKey(watcher.event_identifier))
                return this.watchers[watcher.event_identifier].Remove(watcher);
            else
                return false;
        }


        public List<String> RaiseEvent(string event_identifier, params object[] o)
        {
            return RaiseEvent("program", event_identifier, o);
        }

        public List<String> RaiseEvent(string event_emitter, string event_identifier, params object[] o)
        {
            List<String> calledEvents = new List<string>();
            if (this.watchers.ContainsKey(event_identifier))
                foreach (Watcher watcher in this.watchers[event_identifier].ToArray())
                    if (watcher.Watch(this, event_emitter, event_identifier, o))
                        calledEvents.Add(watcher.event_identifier);

            return calledEvents;
        }

        

    }
}
