using Monytor.Core.Models;
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
        public ViewCollection Get(Guid id) {
            return _repository.Load(id);
        }

        public IEnumerable<ViewCollection> GetOverview() {
            return _repository.LoadOverview();
        }

        public void Set(ViewCollection config) {           
            _repository.Save(config);
        }
    }
}