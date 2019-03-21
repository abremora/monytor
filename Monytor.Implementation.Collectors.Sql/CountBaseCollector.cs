
using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors.Sql {

    public abstract class CountBaseCollector : Collector {
        public string TableName { get; set; }
        public string WhereClause { get; set; }
        public string ConnectionString { get; set; }
        public override string GroupName { get; set; }

        public CountBaseCollector() {
            GroupName = "Count";
        }        
    }
}