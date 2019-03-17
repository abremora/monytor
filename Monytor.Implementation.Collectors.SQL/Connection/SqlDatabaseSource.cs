using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors.SQL {
    public class SqlDatabaseSource : Source {
        public DatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
