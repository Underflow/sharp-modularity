using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modularity
{
    class Dependancy
    {
        public string name { get; private set; }
        public Predicate<float> version_predicate { get; private set; }

        public Dependancy(string name, Predicate<float> version_predicate)
        {
            this.name = name;
            this.version_predicate = version_predicate;
        }

        public Dependancy(string name)
        {
            this.name = name;
            this.version_predicate = x => true;
        }
    }
}
