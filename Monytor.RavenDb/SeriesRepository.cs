using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;

namespace Monytor.RavenDb {
    public class SeriesRepository : ISeriesRepository {

        private readonly IDocumentStore _store;

        public SeriesRepository(IDocumentStore store) {
            _store = store;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            using (var session = _store.OpenSession()) {
                var result = session.Query<Series, SeriesIndex>()
                    .Select(x => new { x.Group, x.Tag })
                    .Distinct()
                    .ToList();


                var z = result.GroupBy(x => x.Group).ToDictionary(g => g.Key, g => g.Select(x => x.Tag));
                return z;
            }
        }

        public Series GetSeries(int id) {
            using (var session = _store.OpenSession()) {
                return session.Load<Series>(id); ;
            }
        }

        public IEnumerable<Series> GetSeries(SeriesQuery queryModel) {
            using (var session = _store.OpenSession()) {
                var query = session.Query<Series, SeriesIndex>()
                    .Where(x => x.Time >= queryModel.Start
                    && x.Time <= queryModel.End
                    && x.Tag == queryModel.Tag
                    && x.Group == queryModel.Group);

                if (queryModel.OrderBy == Ordering.Ascending) {
                    query.OrderBy(x => x.Time);
                }
                else {
                    query.OrderByDescending(x => x.Time);
                }

                query.Take(queryModel.MaxValues);

                return query;
            }
        }

        public void Store(Series series) {
            using (var session = _store.OpenSession()) {
                session.Store(series);
                session.SaveChanges();
            }
        }
    }
}
