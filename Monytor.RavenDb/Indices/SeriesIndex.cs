using System;
using System.Linq;
using Monytor.Core.Models;
using Raven.Client.Indexes;

namespace Monytor.RavenDb.Indices {
    public class SeriesIndex : AbstractIndexCreationTask<Series> {
        public class Result {
            public string Tag { get; set; }
            public string Group { get; set; }
            public DateTime Time { get; set; }
            public string Value { get; set; }
        }

        public SeriesIndex() {
            Map = series => from serie in series
                            select new Result {
                                Tag = serie.Tag,
                                Group = serie.Group,
                                Time = serie.Time,
                                Value = serie.Value
                            };
        }
    }
}