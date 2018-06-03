using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;

namespace Monytor.Implementation.Collectors {
    public class CollectionCollectorBehavior : CollectorBehavior<CollectionCollector> {
        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as CollectionCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;
            var store = RavenHelper.CreateStore(collectorTyped.Source.Url, collectorTyped.Source.Database);

            int result = 0;

            using (var session = store.OpenSession()) {
                result = session.Advanced
                    .DocumentQuery<RavenJObject>("Raven/DocumentsByEntityName")
                    .WhereEquals("Tag", collectorTyped.CollectionName)
                    .Count();
            }

            var series = new Series {
                Id = Series.CreateId(collectorTyped.CollectionName, collectorTyped.GroupName, currentTime),
                Tag = collectorTyped.CollectionName,
                Group = collectorTyped.GroupName,
                Time = currentTime,
                Value = result.ToString()
            };

            yield return series;
        }
    }
}