
using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors.SQL {

    public class CountCollector : Collector {
        public string TableName { get; set; }
        public string WhereClause { get; set; }
        public SqlDatabaseSource  Source { get; set; }
        public override string GroupName { get; set; }

        public CountCollector() {
            GroupName = "Count";
        }        
    }
}