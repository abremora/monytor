using System;

namespace Monytor.PostgreSQL.Results {
    internal class SeriesByMeanResult {
        public string Group { get; set; }
        public string Tag { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }
}