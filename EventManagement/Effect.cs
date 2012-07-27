using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    class Effect
    {
        //Method called when the event is raised
        public delegate void EffectAction(params object[] o);
        private EffectAction action;
        private Type[] param_types;

        public Effect(EffectAction action, Type[] param_types)
        {
            this.param_types = param_types;
            this.action = action;
        }

        public static Effect Create(Action action)
        {
            return new Effect(x => action(), new Type[] {});
        }
        public static Effect Create<T1>(Action<T1> action)
        {
            return new Effect(x => action((T1)x[0]), new Type[] {typeof(T1)});
        }
        public static Effect Create<T1, T2>(Action<T1, T2> action)
        {
            return new Effect(x => action((T1)x[0],
                                          (T2)x[1]),
                              new Type[] {
                                typeof(T1),
                                typeof(T2)});
        }
        public static Effect Create<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return new Effect(x => action((T1)x[0],
                                          (T2)x[1],
                                          (T3)x[2]),
                              new Type[] {
                                  typeof(T1),
                                  typeof(T2),
                                  typeof(T3)});
        }
        public static Effect Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return new Effect(x => action((T1)x[0], 
                                          (T2)x[1],
                                          (T3)x[2],
                                          (T4)x[3]),
                              new Type[] {
                                  typeof(T1),
                                  typeof(T2),
                                  typeof(T3),
                                  typeof(T4) });
        }
        public static Effect Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return new Effect(x => action((T1)x[0],
                              (T2)x[1],
                              (T3)x[2],
                              (T4)x[3],
                              (T5)x[4]),
                  new Type[] {
                                  typeof(T1),
                                  typeof(T2),
                                  typeof(T3),
                                  typeof(T4),
                                  typeof(T5)
                              });
        }

        private int CheckTypes(params object[] o)
        {
            if (param_types.Count() != o.Count())
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < o.Count(); i++)
                    if (o[i].GetType() != param_types[i])
                        return -2;
                return 1;
            }
        }


        public void Raise(EventSystem EventSystem, string event_identifier, params object[] o)
        {  
                switch(CheckTypes(o))
                {
                    case 1:
                        action(o);
                        break;
                    case -1:
                        EventSystem.RaiseEvent("module_manager", "error", string.Format("Error when calling an effect taking {0} parameters with {1} parameters. Event : {2}", param_types.Count(), o.Count(), event_identifier));
                        break;
                    case -2:
                        string out_data_types = "";
                        string in_data_types = "";

                        for (int i = 0; i < o.Count(); i++)
                        {
                            in_data_types = in_data_types == "" ? param_types[i].Name : in_data_types + ", " + param_types[i].GetType().Name;
                            out_data_types = out_data_types == "" ? o[i].GetType().Name : out_data_types + ", " + o[i].GetType().Name;
                        }

                        out_data_types = out_data_types == "" ? "no parameters" : out_data_types;


                        EventSystem.RaiseEvent("module_manager", "error", string.Format("Error while spreading the event {0} with data types : ({1}) instead of ({2})", event_identifier, out_data_types, in_data_types));
                        break;
                    default:
                        break;
                }
            }
        }
    }
