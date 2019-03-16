using System;

namespace Monytor.Core.Models {
    public class SeriesQuery {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int MaxValues { get; set; } = 1024;
        public string Group { get; set; }
        public string Tag { get; set; }
        public Ordering OrderBy { get; set; } = Ordering.Ascending;
        public string MeanValueType { get; set; }
    }
}