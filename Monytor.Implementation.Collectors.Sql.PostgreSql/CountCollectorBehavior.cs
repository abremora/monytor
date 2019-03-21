using System.Data;

namespace Monytor.Implementation.Collectors.Sql.PostgreSql {
    public class CountCollectorBehavior : CountBaseCollectorBehavior<CountCollector> {
        public override IDbConnection CreateDbConnection(string connectionString) {
            return new Npgsql.NpgsqlConnection(connectionString);
        }               
    }
}