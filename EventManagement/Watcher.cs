using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    class Watcher
    {
        public delegate bool WatcherCondition(params object[] o);
        private WatcherCondition condition;
        private Effect effect;

        public string event_emitter { get; private set; }
        public string event_identifier { get; private set; }

        public Watcher(string event_emitter, string event_identifier, Effect effect, WatcherCondition condition)
        {
            this.effect = effect;
            this.condition = condition;
            this.event_identifier = event_identifier;
            this.event_emitter = event_emitter;
        }

        public Watcher(string event_identifier, Effect effect, WatcherCondition condition)
        {
            this.effect = effect;
            this.condition = condition;
            this.event_identifier = event_identifier;
            this.event_emitter = "*";
        }

        public bool Watch(EventSystem EventSystem, string event_emitter, string event_identifier, params object[] o)
        {
            //If the message type correspond to the type of message watched by the watcher
            if (event_identifier == this.event_identifier)
            {
                if (this.event_emitter == "*" || this.event_emitter == event_emitter)
                {
                    //If the watcher's condition is validated
                    if (condition != null && condition(o))
                    {
                        effect.Raise(EventSystem, event_identifier, o);
                        return true;
                    }
                }
            }
            return false;
        }


        public bool Watch(EventSystem EventSystem, string event_identifier, params object[] o)
        {
            //If the message type correspond to the type of message watched by the watcher
            if (event_identifier == this.event_identifier)
            {
                //If the watcher's condition is validated
                if (condition != null && condition(o))
                {
                    effect.Raise(EventSystem, event_identifier, o);
                    return true;
                }
            }
            return false;
        }


    }
}
