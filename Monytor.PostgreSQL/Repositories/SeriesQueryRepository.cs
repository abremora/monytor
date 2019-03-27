using Marten;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Monytor.PostgreSQL {
    public partial class SeriesQueryRepository : ISeriesQueryRepository {
        private readonly IDocumentStore _store;

        public SeriesQueryRepository(IDocumentStore store) {
            _store = store;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            using (var session = _store.QuerySession()) {
                var result = session.Query<TagGroupResult>(@"SELECT  json_build_object('Group', data->'Group', 'Tag' , data->'Tag') 
	                                                    FROM public.mt_doc_series
	                                                    GROUP BY data->'Group', data->'Tag'
	                                                    ORDER BY data->'Group', data->'Tag';")
                                        .ToList();

                return result.GroupBy(g => g.Group)
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
            using (var session = _store.OpenSession()) {
                string orderDirection = queryModel.OrderBy == Ordering.Ascending ? "asc" : "desc";

                var series = session.Query<SeriesByMeanResult>(
                    $@"select json_build_object('Group', data->>'Group', 'Tag', data->>'Tag', 'Time', CAST(data->>'Time' as date), 'Value' ,(SUM( CAST(data->>'Value' as real )) / COUNT(*)))
                    FROM public.mt_doc_series AS d
                    WHERE (date_trunc('hour', CAST(data->>'Time' as timestamp)) >= :Start
	                    AND date_trunc('hour', CAST(data->>'Time' as timestamp)) <= :End
	                    AND d.data ->> 'Tag' = :Tag
	                    AND d.data ->> 'Group' = :Group)
                    GROUP BY data->>'Group', data->>'Tag', CAST(data->>'Time' as date)
                    ORDER BY CAST(data->>'Time' as date) {orderDirection}
                   LIMIT :Limit",
                    new {
                        queryModel.Group,
                        queryModel.Tag,
                        queryModel.Start,
                        queryModel.End,
                        Limit = queryModel.MaxValues
                    });

                return series.Select(group => new Series {
                    Group = group.Group,
                    Tag = group.Tag,
                    Time = group.Time,
                    Value = group.Value.ToString(CultureInfo.InvariantCulture)
                });
            }
        }

        public IEnumerable<Series> GetSeriesByHourMean(SeriesQuery queryModel) {
            using (var session = _store.OpenSession()) {
                string orderDirection = queryModel.OrderBy == Ordering.Ascending ? "asc" : "desc";

                var series = session.Query<SeriesByMeanResult>(
                    $@"select json_build_object('Group', data->>'Group', 'Tag', data->>'Tag', 'Time', date_trunc('hour',CAST(data->>'Time' as timestamp)), 'Value' ,(SUM( CAST(data->>'Value' as real )) / COUNT(*)))
                    FROM public.mt_doc_series AS d
                    WHERE (date_trunc('hour', CAST(data->>'Time' as timestamp)) >= :Start
	                    AND date_trunc('hour', CAST(data->>'Time' as timestamp)) <= :End
	                    AND d.data ->> 'Tag' = :Tag
	                    AND d.data ->> 'Group' = :Group)
                    GROUP BY data->>'Group', data->>'Tag', date_trunc('hour',CAST(data->>'Time' as timestamp))
                    ORDER BY date_trunc('hour',CAST(data->>'Time' as timestamp)) {orderDirection}
                   LIMIT :Limit",
                    new {
                        queryModel.Group,
                        queryModel.Tag,
                        queryModel.Start,
                        queryModel.End,
                        Limit = queryModel.MaxValues
                    });
                return series.Select(group => new Series {
                    Group = group.Group,
                    Tag = group.Tag,
                    Time = group.Time,
                    Value = group.Value.ToString(CultureInfo.InvariantCulture)
                });               
            }
        }
    }
}
