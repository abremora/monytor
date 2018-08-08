using Monytor.Core.Services;
using Monytor.WebApi.Controllers;
using System;
using System.Collections.Generic;

namespace Monytor.Domain.Services {
    public class ViewCollectionService : IViewCollectionService {
        private readonly IViewCollectionRepository _repository;

        public ViewCollectionService(IViewCollectionRepository repository) {
            _repository = repository;
        }

        public ViewCollection Get(string id) {
            var internalId = ViewCollection.AddInternalId(id);
            return _repository.Load(internalId);
        }

        public IEnumerable<ViewCollection> GetOverview() {
            var overview = _repository.LoadOverview();
            
            foreach(var view in overview) {
                view.RemoveInternalId();
            }

            return overview;
        }

        public void Set(ViewCollection config) {           
            _repository.Save(config);
        }
    }
}