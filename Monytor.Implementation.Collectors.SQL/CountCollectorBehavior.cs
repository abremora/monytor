using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Collections.Generic;

namespace Monytor.Implementation.Collectors.SQL {
    public class CountCollectorBehavior : CollectorBehavior<CountCollector> {
        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as CountCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;

            using (var connection = DbConnectionFactory.CreateDbConnection(collectorTyped.Source)) {
                connection.Open();
                using (var command = connection.CreateCommand()) {
                    command.CommandText = $"SELECT COUNT(*) FROM {collectorTyped.TableName}";

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