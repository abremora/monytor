using System;
using System.Collections.Generic;

namespace Monytor.WebApi.Controllers {
    public class ViewCollection {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<View> Views { get; set; }

        public static string CreateId(string name) {
            return $"{nameof(ViewCollection)}/{name}";
        }
    }

    public class View {
        public int Position { get; set; }
        public string Group { get; set; }
        public string Tag { get; set; }
        public TimeSpan Range { get; set; }
    }
}