using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Configurations;
using System;

namespace Monytor.Domain.Factories {
    public static class SqlCollectorFactory {
        public static Collector CreateCountCollector(SqlCollectorSourceProvider sourceProvider) {
            switch (sourceProvider) {
                case SqlCollectorSourceProvider.PostgreSql:
                    return new Implementation.Collectors.Sql.PostgreSql.CountCollector();
                case SqlCollectorSourceProvider.Oracle:
                    return new Implementation.Collectors.Sql.Oracle.CountCollector();
                case SqlCollectorSourceProvider.MySql:
                    return new Implementation.Collectors.Sql.MySql.CountCollector();
                case SqlCollectorSourceProvider.MsSql:
                    return new Implementation.Collectors.Sql.MsSql.CountCollector();
                default:
                    throw new NotSupportedException($"{sourceProvider} has no support for CountCollector");
            }
        }
    }
}
