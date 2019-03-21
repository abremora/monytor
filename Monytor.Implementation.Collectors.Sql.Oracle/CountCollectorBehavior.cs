using System.Data;

namespace Monytor.Implementation.Collectors.Sql.Oracle {
    public class CountCollectorBehavior : CountBaseCollectorBehavior<CountCollector> {
        public override IDbConnection CreateDbConnection(string connectionString) {
            return new global::Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
        }               
    }
}