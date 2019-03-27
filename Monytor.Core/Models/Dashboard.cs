using System;
using System.Collections.Generic;

namespace Monytor.Core.Models {

    public class Dashboard {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<View> Views { get; set; }

        public static bool HasPrefix(string id) {
            return id.StartsWith(nameof(Dashboard) + "/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string CreateId(string id) {
            return $"{nameof(Dashboard)}/{id}";
        }

        public static string CreateId() {
            return CreateId(Guid.NewGuid().ToString());
        }
    }
}