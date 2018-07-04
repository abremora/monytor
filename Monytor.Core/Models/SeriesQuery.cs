using System;

namespace Monytor.Core.Models {
    public class SeriesQuery {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int MaxValues { get; set; }
        public string Group { get; set; }
        public string Tag { get; set; } 
        public Ordering OrderBy { get; set; }
    }

    public enum Ordering {
        Ascending,
        Descanding
    }
}