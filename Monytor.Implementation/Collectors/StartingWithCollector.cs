using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors {

    public class StartingWithCollector : Collector {
        public string StartingWith { get; set; }
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; }

        public StartingWithCollector() {
            GroupName = "StartingDocumentId";
        }        
    }
}