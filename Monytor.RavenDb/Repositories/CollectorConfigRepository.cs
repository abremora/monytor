using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;

namespace Monytor.PostgreSQL {
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