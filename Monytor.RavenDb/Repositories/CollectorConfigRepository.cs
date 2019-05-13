using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.RavenDb.Repositories {
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

        public void Delete(string id) {
            _unitOfWork.Session.Delete(id);
        }

        public void Delete(CollectorConfigStored collectorConfig) {
            _unitOfWork.Session.Delete(collectorConfig);
        }
    }
}