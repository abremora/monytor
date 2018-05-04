using Raven.Abstractions.Data;
using System;
using Raven.Client;
using System.Collections.Generic;
using Raven.Json.Linq;
using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;

namespace Monytor.Implementation.Collectors {
    public class AllCollectionCollectorBehavior : CollectorBehavior<AllCollectionCollector> {

        public override IEnumerable<Serie> Run(Collector collector) {
            var collectorTyped = collector as AllCollectionCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;
            var source = RavenHelper.CreateStore(collectorTyped.Source.Url, collectorTyped.Source.Database);

            FacetResults results;
            using (var session = source.OpenSession()) {
                results = session.Query<RavenJObject>("Raven/DocumentsByEntityName")
                .AggregateBy(x => x.Tag)
                .CountOn(x => x.Tag)
                .ToList();
            }

            foreach (var result in results.Results["Tag"].Values) {
                var serie = new Serie {
                    Id = Serie.CreateId(result.Range, collectorTyped.GroupName, currentTime),
                    Tag = result.Range,
                    Group = collectorTyped.GroupName,
                    Time = currentTime,
                    Value = result.Count.ToString()
                };

                yield return serie;
            }
        }
    }
}