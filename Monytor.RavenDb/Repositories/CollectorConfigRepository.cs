using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;

namespace Monytor.RavenDb {
    public class CollectorConfigRepository : ICollectorConfigRepository {
        private readonly IDocumentStore _store;

        public CollectorConfigRepository(IDocumentStore store) {
            _store = store;
        }

        public CollectorConfigStored Get(string id) {
            using (var session = _store.OpenSession()) {
                return session.Load<CollectorConfigStored>(id);
            }
        }

        public void Store(CollectorConfigStored collectorConfig) {
            using (var session = _store.OpenSession()) {                
                session.Store(collectorConfig);
                session.SaveChanges();
            }
        }
    }
}