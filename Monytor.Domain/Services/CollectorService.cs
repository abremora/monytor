using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using System.Collections.Generic;

namespace Monytor.Domain.Services {
    public class CollectorService : ICollectorService {
        private readonly ISerieRepository _repository;

        public CollectorService(ISerieRepository repository) {
            _repository = repository;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            return _repository.GetGroupValueSummary();
        }

        public Serie GetSerie(int id) {
            return _repository.GetSerie(id);
        }

        public IEnumerable<Serie> GetSeries(SerieQuery query) {
            return _repository.GetSeries(query);
        }

        public void Set(Serie serie) {
            _repository.Store(serie);
        }
    }
}