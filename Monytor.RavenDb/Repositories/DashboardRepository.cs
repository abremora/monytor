using System.Collections.Generic;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;

namespace Monytor.RavenDb {
    public class DashboardRepository : IDashboardRepository {
        private readonly IDocumentStore _store;

        public DashboardRepository(IDocumentStore store) {
            _store = store;
        }

        public Dashboard Load(string id) {
            using (var session = _store.OpenSession()) {
                return session.Load<Dashboard>(id);
            }
        }

        public IEnumerable<Dashboard> LoadOverview() {
            using (var session = _store.OpenSession()) {
                return session.Query<Dashboard>()
                    .Take(1024).ToList();
            }
        }

        public void Save(Dashboard config) {
            using (var session = _store.OpenSession()) {                
                session.Store(config);
                session.SaveChanges();
            }
        }
    }
}