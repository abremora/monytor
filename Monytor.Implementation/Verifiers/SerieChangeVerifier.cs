using Monytor.Core.Configurations;
using System;

namespace Monytor.Implementation.Verifiers {
    public class SeriesChangeVerifier : Verifier {
        public string Group { get; set; }
        public string Tag { get; set; }
        public TimeSpan TimeInterval { get; set; }
    }
}
