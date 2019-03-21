using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Monytor.Implementation.Collectors.Sql {
    public abstract class CountBaseCollectorBehavior<TCountCollector> : CollectorBehavior<TCountCollector> where TCountCollector : CountBaseCollector{
        public abstract IDbConnection CreateDbConnection(string connectionString);

        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as TCountCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;

            using (var connection = CreateDbConnection(collectorTyped.ConnectionString)) {
                connection.Open();
                using (var command = connection.CreateCommand()) {
                    command.CommandText = $"SELECT COUNT(*) FROM {collectorTyped.TableName}";

                    if (!string.IsNullOrWhiteSpace(collectorTyped.WhereClause)) {
                        command.CommandText += $" {collectorTyped.WhereClause}";
                    }

                    var rowCount = command.ExecuteScalar();
                    yield return new Series {
                        Id = Series.CreateId(collectorTyped.TableName, collectorTyped.GroupName, currentTime),
                        Tag = collectorTyped.TableName,
                        Group = collectorTyped.GroupName,
                        Time = currentTime,
                        Value = rowCount.ToString()
                    };
                }
            }
        }
    }
}