using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors.RavenDb {
    public class AllCollectionCollector : Collector {
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; }

        public AllCollectionCollector() {
            GroupName = "Collection";
        }
    }
}
