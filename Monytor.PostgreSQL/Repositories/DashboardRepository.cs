using System.Collections.Generic;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL.Repositories {
    public class DashboardRepository : IDashboardRepository {
        private readonly UnitOfWork _unitOfWork;

        public DashboardRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public Dashboard Get(string id) {
            return _unitOfWork.Session.Load<Dashboard>(id);
        }
           
        public IEnumerable<Dashboard> LoadOverview() {
            // ToDo: Query-Repository
            using (var session = _unitOfWork.Store.QuerySession()) {
                return _unitOfWork.DirtyTrackedSession.Query<Dashboard>()
                        .Take(1024).ToList();
            }
        }

        public void Store(Dashboard config) {
            _unitOfWork.Session.Store(config);
        }
    }
}