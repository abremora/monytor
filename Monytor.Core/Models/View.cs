using System;

namespace Monytor.WebApi.Controllers {
    public class View {
        public int Position { get; set; }
        public string Group { get; set; }
        public string Tag { get; set; }
        public TimeSpan Range { get; set; }
        public string ChartType { get; set; }
        public string MeanValueType { get; set; }
    }
}