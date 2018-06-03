using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using System.Collections.Generic;

namespace Monytor.Domain.Services {
    public class CollectorService : ICollectorService {
        private readonly ISeriesRepository _repository;

        public CollectorService(ISeriesRepository repository) {
            _repository = repository;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            return _repository.GetGroupValueSummary();
        }

        public Series GetSerie(int id) {
            return _repository.GetSeries(id);
        }

        public IEnumerable<Series> GetSeries(SeriesQuery query) {
            return _repository.GetSeries(query);
        }

        public void Set(Series series) {
            _repository.Store(series);
        }
    }
}