using Marten;
using Marten.Linq.MatchesSql;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Monytor.PostgreSQL {
    public class SeriesQueryRepository : ISeriesQueryRepository {
        private readonly IDocumentStore _store;

        public SeriesQueryRepository(IDocumentStore store) {
            _store = store;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            using (var session = _store.QuerySession()) {
                var result = session.Query<TagGroupResult>(@"SELECT  json_build_object('Group', data->'Group', 'Tag' , data->'Tag') 
	                                                    FROM public.mt_doc_series
	                                                    group by data->'Group', data->'Tag'
	                                                    order by data->'Group', data->'Tag';")
                                        .ToList();

                return result.GroupBy(g => g.Group)
                     .ToDictionary(g => g.Key, g => g.Select(x => x.Tag));
            }
        }

        public Series GetSeries(int id) {
            using (var session = _store.QuerySession()) {
                return session.Load<Series>(id);
            }
        }

        public IEnumerable<Series> GetSeries(SeriesQuery queryModel) {
            using (var session = _store.QuerySession()) {
                var query = session.Query<Series>()
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
            using (var session = _store.QuerySession()) {
                var query = session.Query<Series>()
                    .Where(x => x.MatchesSql("date_trunc('hour',CAST(data->>'Time' as timestamp)) >= ? AND date_trunc('hour',CAST(data->>'Time' as timestamp)) <= ?", queryModel.Start, queryModel.End)
                    && x.Tag == queryModel.Tag
                    && x.Group == queryModel.Group);

                if (queryModel.OrderBy == Ordering.Ascending) {
                    query = query.OrderBy(x => x.Time);
                }
                else {
                    query = query.OrderByDescending(x => x.Time);
                }
                query = query.Take(queryModel.MaxValues);

                var series = query.ToList();
                var seriesGrouped = series.GroupBy(g => new { g.Group, g.Tag, g.Time.Date });
                foreach (var group in seriesGrouped) {
                    yield return new Series {
                        Group = group.Key.Group,
                        Tag = group.Key.Tag,
                        Time = group.Key.Date,
                        Value = (group.Sum(x => double.Parse(x.Value)) / group.Count()).ToString(CultureInfo.InvariantCulture)
                    };
                }
            }
        }

        public IEnumerable<Series> GetSeriesByHourMean(SeriesQuery queryModel) {
            // ToDo: Query-Repository
            using (var session = _store.QuerySession()) {
                var query = session.Query<Series>()
                    .Where(x => x.MatchesSql("CAST(data->>'Time' as date) >= ? AND CAST(data->>'Time' as date) <= ?", queryModel.Start, queryModel.End)
                    && x.Tag == queryModel.Tag
                    && x.Group == queryModel.Group);

                if (queryModel.OrderBy == Ordering.Ascending) {
                    query = query.OrderBy(x => x.Time);
                }
                else {
                    query = query.OrderByDescending(x => x.Time);
                }
                query = query.Take(queryModel.MaxValues);

                var series = query.ToList();
                series.ForEach(f => f.Time = f.Time.Date.AddHours(f.Time.Hour));
                var seriesGrouped = series.GroupBy(g => new { g.Group, g.Tag, g.Time });
                foreach (var group in seriesGrouped) {
                    yield return new Series {
                        Group = group.Key.Group,
                        Tag = group.Key.Tag,
                        Time = group.Key.Time,
                        Value = (group.Sum(x => double.Parse(x.Value)) / group.Count()).ToString(CultureInfo.InvariantCulture)
                    };
                }
            }
        }

    }
}
