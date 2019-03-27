using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using System;
using System.Collections.Generic;

namespace Monytor.Domain.Factories {
    public class ViewCollectionService : IViewCollectionService {
        private readonly IDashboardRepository _repository;

        public ViewCollectionService(IDashboardRepository repository) {
            _repository = repository;
        }

        public Dashboard Get(string id) {
            if (!Dashboard.HasPrefix(id)) {
                id = Dashboard.CreateId(id);
            }
            return _repository.Get(id);
        }

        public IEnumerable<Dashboard> GetOverview() {
            var overview = _repository.LoadOverview();
            return overview;
        }

        public void Create(Dashboard config) {
            config.Id= Dashboard.CreateId();
            _repository.Store(config);
        }
    }
}