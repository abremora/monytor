namespace Monytor.Contracts.CollectorConfig {
    public class AddSqlCountCollectorToConfigCommand : AddCollectorToConfigCommand {
        public SqlCollectorSourceProvider SourceProvider { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public string WhereClause { get; set; }
    }
}
