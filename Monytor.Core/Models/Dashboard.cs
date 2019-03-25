using System.Collections.Generic;

namespace Monytor.Core.Models {

    public class Dashboard {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<View> Views { get; set; }

        public static string CreateId(string id) {
            return $"{nameof(Dashboard)}/{id}";
        }
    }
}