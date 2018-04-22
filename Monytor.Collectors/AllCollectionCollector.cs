using Raven.Abstractions.Data;
using System;
using Raven.Client;
using System.Collections.Generic;
using Raven.Json.Linq;
using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;

namespace Monytor.Collectors {
    public class AllCollectionCollector : Collector {
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; }

        public AllCollectionCollector() {
            GroupName = "Collection";
        }

        public override IEnumerable<Serie> Run() {
                var currentTime = DateTime.UtcNow;
            var source = RavenHelper.CreateStore(Source.Url, Source.Database);

            FacetResults results;
            using (var session = source.OpenSession()) {
                results = session.Query<RavenJObject>("Raven/DocumentsByEntityName")
                .AggregateBy(x => x.Tag)
                .CountOn(x => x.Tag)
                .ToList();
            }

            foreach (var result in results.Results["Tag"].Values) {
                var serie = new Serie {
                    Id = Serie.CreateId(result.Range, "Collection", currentTime),
                    Tag = result.Range,
                    Group = GroupName,
                    Time = currentTime,
                    Value = result.Count.ToString()
                };

                yield return serie;
            }
        }
    }
}
