using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;

namespace Monytor.RavenDb {
    public class SerieRepository : ISerieRepository {

        private readonly IDocumentStore _store;

        public SerieRepository(IDocumentStore store) {
            _store = store;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            using (var session = _store.OpenSession()) {
                var result = session.Query<Serie, SerieIndex>()
                    .Select(x => new { x.Group, x.Tag })
                    .Distinct()
                    .ToList();


                var z = result.GroupBy(x => x.Group).ToDictionary(g => g.Key, g => g.Select(x => x.Tag));
                return z;
            }
        }

        public Serie GetSerie(int id) {
            using (var session = _store.OpenSession()) {
                return session.Load<Serie>(id); ;
            }
        }

        public IEnumerable<Serie> GetSeries(SerieQuery queryModel) {
            using (var session = _store.OpenSession()) {
                var query = session.Query<Serie, SerieIndex>()
                    .Where(x => x.Time >= queryModel.Start
                    && x.Time <= queryModel.End
                    && x.Tag == queryModel.Tag
                    && x.Group == queryModel.Group);                   

                if(queryModel.OrderBy == Ordering.Ascending) {
                    query.OrderBy(x => x.Time);
                }
                else {
                    query.OrderByDescending(x => x.Time);
                }

                query.Take(queryModel.MaxValues);

                return query;
            }
        }
    }
}
