using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface ISeriesRepository {
        Dictionary<string, IEnumerable<string>> GetGroupValueSummary();
        Series GetSeries(int id);
        IEnumerable<Series> GetSeries(SeriesQuery query);
        void Store(Series series);
    }
}       