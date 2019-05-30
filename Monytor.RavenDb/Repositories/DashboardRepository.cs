using System.Collections.Generic;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.RavenDb.Repositories {
    public class DashboardRepository : IDashboardRepository {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public Dashboard Get(string id) {
            return _unitOfWork.Session.Load<Dashboard>(id);
        }

        public IEnumerable<Dashboard> LoadOverview() {
            // TODO: Currently limited up to 1024
            return _unitOfWork.Session.LoadAll<Dashboard>()
                .OrderBy(x => x.Name);
        }

        public void Store(Dashboard config) {
            _unitOfWork.Session.Store(config);
        }
    }
}