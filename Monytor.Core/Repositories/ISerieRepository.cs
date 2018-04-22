using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface ISerieRepository {
        Dictionary<string, IEnumerable<string>> GetGroupValueSummary();
        Serie GetSerie(int id);
        IEnumerable<Serie> GetSeries(SerieQuery query);
    }
}       