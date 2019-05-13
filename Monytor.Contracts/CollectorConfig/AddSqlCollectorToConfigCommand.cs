namespace Monytor.Contracts.CollectorConfig {
    public abstract class AddSqlCollectorToConfigCommand : AddCollectorToConfigCommand {
        public SqlCollectorSourceProvider SourceProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
