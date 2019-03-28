namespace Monytor.Contracts.CollectorConfig {

    public class AddSqlCountCollectorToConfigCommand : AddSqlCollectorToConfigCommand {
        public string TableName { get; set; }
        public string WhereClause { get; set; }
    }
}
