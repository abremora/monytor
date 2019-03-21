using System.Collections.Generic;

namespace Monytor.Core.Models {
    public class Dashboard {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<View> Views { get; set; }

        public void RemoveInternalId() {
            Id = Id.Remove(0, $"{nameof(Dashboard)}/".Length);
        }

        public static string AddInternalId(string id) {
            return $"{nameof(Dashboard)}/{id}";
        }
    }
}