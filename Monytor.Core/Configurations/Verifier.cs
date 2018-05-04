using System.Collections.Generic;

namespace Monytor.Core.Configurations {
    public abstract class Verifier {
        public List<string> Notifications { get; set; } = new List<string>();
    }
}
