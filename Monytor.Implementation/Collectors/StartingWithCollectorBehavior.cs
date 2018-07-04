using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;
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

            FacetResults results;

            using (var session = store.OpenSession()) {
                results = session.Query<RavenJObject>("Raven/DocumentsByEntityName")                    
                    .Where(x => ((string)x.Tag).StartsWith(collectorTyped.StartingWith))
                    .AggregateBy(x => x.Tag)
                    .CountOn(x => x.Tag)
                    .ToList();
            }

            foreach (var result in results.Results["Tag"].Values) {
                var series = new Series {
                    Id = Series.CreateId(result.Range, collectorTyped.GroupName, currentTime),
                    Tag = result.Range,
                    Group = collectorTyped.GroupName,
                    Time = currentTime,
                    Value = result.Count.ToString()
                };

                yield return series;
            }           
        }
    }
}