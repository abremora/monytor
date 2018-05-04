using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors {

    public class CollectionCollector : Collector {
        public string CollectionName { get; set; }
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; }

        public CollectionCollector() {
            GroupName = "Collection";
        }       
    }
}