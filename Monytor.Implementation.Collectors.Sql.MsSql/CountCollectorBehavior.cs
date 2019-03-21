using System.Data;

namespace Monytor.Implementation.Collectors.Sql.MsSql {
    public class CountCollectorBehavior : CountBaseCollectorBehavior<CountCollector> {
        public override IDbConnection CreateDbConnection(string connectionString) {
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }               
    }
}