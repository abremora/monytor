using Monytor.Core.Repositories;
using Monytor.Core.Services;
using Monytor.WebApi.Controllers;
using System;
using System.Collections.Generic;

namespace Monytor.Domain.Services {
    public class ViewCollectionService : IViewCollectionService {
        private readonly IDashboardRepository _repository;

        public ViewCollectionService(IDashboardRepository repository) {
            _repository = repository;
        }

        public Dashboard Get(string id) {
            var internalId = Dashboard.AddInternalId(id);
            return _repository.Load(internalId);
        }

        public IEnumerable<Dashboard> GetOverview() {
            var overview = _repository.LoadOverview();
            
            foreach(var view in overview) {
                view.RemoveInternalId();
            }

            return overview;
        }

        public void Set(Dashboard config) {           
            _repository.Save(config);
        }
    }
}