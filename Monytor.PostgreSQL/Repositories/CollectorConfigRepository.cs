using Marten;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Monytor.PostgreSQL {
    public class CollectorConfigRepository : ICollectorConfigRepository {
        private readonly UnitOfWork _unitOfWork;

        public CollectorConfigRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork as UnitOfWork;
        }
        
        public CollectorConfigStored Get(string id) {
            return _unitOfWork.Session.Load<CollectorConfigStored>(id);
        }

        public void Store(CollectorConfigStored collectorConfig) {
             _unitOfWork.Session.Store(collectorConfig);
        }
    }
}