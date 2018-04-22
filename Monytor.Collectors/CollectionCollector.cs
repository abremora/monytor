using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;

namespace Monytor.Collectors {
    public class CollectionCollector : Collector {
        public string CollectionName { get; set; }
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; }

        public CollectionCollector() {
            GroupName = "Collection";
        }

        public override IEnumerable<Serie> Run() {
            var currentTime = DateTime.UtcNow;
            var store = RavenHelper.CreateStore(Source.Url, Source.Database);

            int result = 0;

            using (var session = store.OpenSession()) {
                result = session.Advanced
                    .DocumentQuery<RavenJObject>("Raven/DocumentsByEntityName")
                    .WhereStartsWith("Tag", CollectionName)
                    .Count();
            }

            var serie = new Serie {
                Id = Serie.CreateId(CollectionName, "Collection", currentTime),
                Tag = CollectionName,
                Group = GroupName,
                Time = currentTime,
                Value = result.ToString()
            };

            yield return serie;
        }
    }
}