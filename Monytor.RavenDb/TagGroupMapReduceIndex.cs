using Monytor.Core.Models;
using Raven.Client.Indexes;
using System.Linq;

namespace Monytor.RavenDb {
    public class TagGroupMapReduceIndex : AbstractIndexCreationTask<Series, TagGroupMapReduceIndex.Result> {
        public class Result {
            public string Group { get; set; }
            public string Tag { get; set; }            
        }

        public TagGroupMapReduceIndex() {

            Map = series => from s in series
                            select new Result {
                                Group = s.Group,
                                Tag = s.Tag
                            };

            Reduce = results => from result in results
                                group result by new { result.Group, result.Tag } into g
                                select new Result {
                                    Group = g.Key.Group,
                                    Tag = g.Key.Tag                                    
                                };
        }
    }
}