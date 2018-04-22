using Monytor.Core.Models;
using Raven.Client.Indexes;
using System;
using System.Linq;

namespace Monytor.RavenDb
{
    public class SerieIndex : AbstractIndexCreationTask<Serie> {
        public class Result {
            public string Tag { get; set; }
            public string Group { get; set; }
            public DateTime Time { get; set; }
            public string Value { get; set; }
        }

        public SerieIndex() {
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
