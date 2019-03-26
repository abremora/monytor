using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface ISeriesQueryRepository {
        Dictionary<string, IEnumerable<string>> GetGroupValueSummary();
        Series GetSeries(int id);
        IEnumerable<Series> GetSeries(SeriesQuery query);
        IEnumerable<Series> GetSeriesByDayMean(SeriesQuery queryModel);
        IEnumerable<Series> GetSeriesByHourMean(SeriesQuery queryModel);
    }
}       