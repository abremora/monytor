using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;

namespace Monytor.Implementation.Collectors {
    public class StartingWithCollectorBehavior : CollectorBehavior<StartingWithCollector> {
        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as StartingWithCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;
            var store = RavenHelper.CreateStore(collectorTyped.Source.Url, collectorTyped.Source.Database);

            int result = 0;

            using (var session = store.OpenSession()) {
                result = session.Advanced
                    .DocumentQuery<RavenJObject>("Raven/DocumentsByEntityName")
                    .WhereStartsWith("__document_id", collectorTyped.StartingWith)
                    .Count();
            }

            var serie = new Series {
                Id = Series.CreateId(collectorTyped.StartingWith, collectorTyped.GroupName, currentTime),
                Tag = collectorTyped.StartingWith,
                Group = collectorTyped.GroupName,
                Time = currentTime,
                Value = result.ToString()
            };

            yield return serie;
        }
    }
}