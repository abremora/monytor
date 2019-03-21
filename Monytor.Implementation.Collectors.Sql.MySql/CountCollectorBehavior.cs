using System.Data;

namespace Monytor.Implementation.Collectors.Sql.MySql {
    public class CountCollectorBehavior : CountBaseCollectorBehavior<CountCollector> {
        public override IDbConnection CreateDbConnection(string connectionString) {
            return new global::MySql.Data.MySqlClient.MySqlConnection(connectionString);
        }               
    }
}