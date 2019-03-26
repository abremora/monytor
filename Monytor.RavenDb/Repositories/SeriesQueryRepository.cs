using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Monytor.RavenDb {
    public class SeriesQueryRepository : ISeriesQueryRepository {
        private readonly IDocumentStore _store;

        public SeriesQueryRepository(IDocumentStore store) {
            _store = store;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            using (var session = _store.OpenSession()) {
                return session.Query<TagGroupMapReduceIndex.Result, TagGroupMapReduceIndex>()
                    .OrderBy(x => x.Group)
                    .ThenBy(x => x.Tag)
                    .Take(1024)
                    .ToList()
                    .GroupBy(x => x.Group)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Tag));
            }
        }

        public Series GetSeries(int id) {
            using (var session = _store.OpenSession()) {
                return session.Load<Series>(id);
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
                    query = query.OrderBy(x => x.Time);
                }
                else {
                    query = query.OrderByDescending(x => x.Time);
                }

                query = query.Take(queryModel.MaxValues);

                return query;
            }
        }

        public IEnumerable<Series> GetSeriesByDayMean(SeriesQuery queryModel) {
            using (var session = _store.OpenSession()) {
                var query = session.Query<SeriesByDayIndex.Result, SeriesByDayIndex>()
                    .Where(x => x.Date >= queryModel.Start
                    && x.Date <= queryModel.End
                    && x.Tag == queryModel.Tag
                    && x.Group == queryModel.Group);

                if (queryModel.OrderBy == Ordering.Ascending) {
                    query = query.OrderBy(x => x.Date);
                }
                else {
                    query = query.OrderByDescending(x => x.Date);
                }

                query = query.Take(queryModel.MaxValues);

                return query.ToList().Select(x => new Series {
                    Group = x.Group,
                    Tag = x.Tag,
                    Time = x.Date,
                    Value = x.MeanValue.ToString(CultureInfo.InvariantCulture)
                });
            }
        }

        public IEnumerable<Series> GetSeriesByHourMean(SeriesQuery queryModel) {
            using (var session = _store.OpenSession()) {
                var query = session.Query<SeriesByHourIndex.Result, SeriesByHourIndex>()
                    .Where(x => x.Date >= queryModel.Start
                    && x.Date <= queryModel.End
                    && x.Tag == queryModel.Tag
                    && x.Group == queryModel.Group);

                if (queryModel.OrderBy == Ordering.Ascending) {
                    query = query.OrderBy(x => x.Date);
                }
                else {
                    query = query.OrderByDescending(x => x.Date);
                }

                query = query.Take(queryModel.MaxValues);

                return query.ToList().Select(x => new Series {
                    Group = x.Group,
                    Tag = x.Tag,
                    Time = x.Date,
                    Value = x.MeanValue.ToString(CultureInfo.InvariantCulture)
                });
            }
        }
    }
}
