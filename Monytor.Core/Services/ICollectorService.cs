using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Services {
    public interface ICollectorService {
        IEnumerable<Serie> GetSeries(SerieQuery query);
        Serie GetSerie(int id);
        Dictionary<string, IEnumerable<string>> GetGroupValueSummary();
        void Set(Serie serie);
    }
}