using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Configurations {
    public abstract class CollectorBehaviorBase : Behavior {
        public abstract IEnumerable<Serie> Run(Collector collector);
    }
}
