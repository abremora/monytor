using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monytor.Domain.Services {
    public class CollectorConfigService : ICollectorConfigService {
        private readonly ICollectorConfigRepository _collectorConfigRespository;

        public CollectorConfigService(ICollectorConfigRepository collectorConfigRespository) {
            _collectorConfigRespository = collectorConfigRespository;
        }

        public CollectorConfigStored Get(string id) {
            return _collectorConfigRespository.Get(id);
        }

        public void Create(CollectorConfigStored collectorConfig) {
            _collectorConfigRespository.Store(collectorConfig);
        }
    }
}
