using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors.Sql;
using System;

namespace Monytor.Domain.Factories {
    public static class SqlCollectorFactory {
        public static Collector CreateCollector(AddSqlCollectorToConfigCommand addSqlCollectorCommand) {
            switch (addSqlCollectorCommand) {
                case AddSqlCountCollectorToConfigCommand command:
                    return CreateCountCollector(command);
                default:
                    throw new NotSupportedException($"{addSqlCollectorCommand?.GetType()} is not supported yet.");
            }
        }

        public static Collector CreateCountCollector(AddSqlCountCollectorToConfigCommand command) {
            CountBaseCollector collector = null;
            switch (command.SourceProvider) {
                case SqlCollectorSourceProvider.PostgreSql:
                    collector = new Implementation.Collectors.Sql.PostgreSql.CountCollector();
                    break;
                case SqlCollectorSourceProvider.Oracle:
                    collector = new Implementation.Collectors.Sql.Oracle.CountCollector();
                    break;
                case SqlCollectorSourceProvider.MySql:
                    collector = new Implementation.Collectors.Sql.MySql.CountCollector();
                    break;
                case SqlCollectorSourceProvider.MsSql:
                    collector = new Implementation.Collectors.Sql.MsSql.CountCollector();
                    break;
                default:
                    throw new NotSupportedException($"{command.SourceProvider} has no support for the CountCollector");
            }

            collector.ConnectionString = command.ConnectionString;
            collector.TableName = command.TableName;
            collector.WhereClause = command.WhereClause;

            return collector;
        }
    }
}
