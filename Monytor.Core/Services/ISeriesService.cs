using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Services {

    public interface ISeriesService {
        IEnumerable<Series> GetSeries(SeriesQuery query);
        Series GetSerie(int id);
        Dictionary<string, IEnumerable<string>> GetGroupValueSummary();
        void Create(Series series);
    }
}