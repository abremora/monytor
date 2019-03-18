using System;

namespace Monytor.Core.Models {
    public class View {
        public int Position { get; set; }
        public string Group { get; set; }
        public string Tag { get; set; }
        public TimeSpan Range { get; set; }
        public string ChartType { get; set; }
        public string MeanValueType { get; set; }
    }
}