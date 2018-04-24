using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;

namespace Monytor.Collectors {
    public class StartingWithCollector : Collector {
        public string StartingWith { get; set; }
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; }

        public StartingWithCollector() {
            GroupName = "StartingDocumentId";
        }
        public override IEnumerable<Serie> Run() {
            var currentTime = DateTime.UtcNow;
            var store = RavenHelper.CreateStore(Source.Url, Source.Database);

            int result = 0;

            using (var session = store.OpenSession()) {
                result = session.Advanced
                    .DocumentQuery<RavenJObject>("Raven/DocumentsByEntityName")
                    .WhereStartsWith("__document_id", StartingWith)
                    .Count();
            }

            var serie = new Serie {
                Id = Serie.CreateId(StartingWith, GroupName, currentTime),
                Tag = StartingWith,
                Group = GroupName,
                Time = currentTime,
                Value = result.ToString()
            };

            yield return serie;
        }
    }
}