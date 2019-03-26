using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;

namespace Monytor.RavenDb {
    public class CollectorConfigRepository : ICollectorConfigRepository {
        private readonly UnitOfWork _unitOfWork;

        public CollectorConfigRepository(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public CollectorConfigStored Get(string id) {
            return _unitOfWork.Session.Load<CollectorConfigStored>(id);
        }

        public void Store(CollectorConfigStored collectorConfig) {
            _unitOfWork.Session.Store(collectorConfig);
        }
    }
}