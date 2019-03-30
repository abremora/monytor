using System.Threading.Tasks;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.RavenDb.Indices;
using Raven.Client;

namespace Monytor.RavenDb.Repositories {
    public class CollectorConfigQueryRepository : ICollectorConfigQueryRepository {
        private readonly IDocumentStore _documentStore;

        public CollectorConfigQueryRepository(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public CollectorConfigStored Get(string id) {
            using(var session = _documentStore.OpenSession()) {
                return session.Load<CollectorConfigStored>(id);
            }
        }

        public async Task<CollectorConfigStored> GetAsync(string id) {
            using(var session = _documentStore.OpenAsyncSession()) {
                return await session.LoadAsync<CollectorConfigStored>(id);
            }
        }

        public CollectorConfigStored GetByAgentId(string schedulerAgentId) {
            using(var session = _documentStore.OpenSession()) {
                return session.Query<CollectorConfigStored, CollectorConfigIndex>()
                    .FirstOrDefault(x => x.SchedulerAgentId.Equals(schedulerAgentId));
            }
        }

        public async Task<CollectorConfigStored> GetByAgentIdAsync(string schedulerAgentId) {
            using(var session = _documentStore.OpenAsyncSession()) {
                return await session.Query<CollectorConfigStored, CollectorConfigIndex>()
                    .FirstOrDefaultAsync( x => x.SchedulerAgentId.Equals(schedulerAgentId));
            }
        }
    }
}