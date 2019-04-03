using System.Linq;
using System.Threading.Tasks;
using Marten;
using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL.Repositories {
    public class CollectorConfigQueryRepository : ICollectorConfigQueryRepository {
        private readonly IDocumentStore _documentStore;

        public CollectorConfigQueryRepository(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public CollectorConfigStored Get(string id) {
            using(var session = _documentStore.QuerySession()) {
                return session.Load<CollectorConfigStored>(id);
            }
        }

        public async Task<CollectorConfigStored> GetAsync(string id) {
            using(var session = _documentStore.QuerySession()) {
                return await session.LoadAsync<CollectorConfigStored>(id);
            }
        }

        public CollectorConfigStored GetByAgentId(string schedulerAgentId) {
            using(var session = _documentStore.QuerySession()) {
                return session.Query<CollectorConfigStored>()
                    .FirstOrDefault(x => x.SchedulerAgentId.Equals(schedulerAgentId));
            }
        }

        public async Task<CollectorConfigStored> GetByAgentIdAsync(string schedulerAgentId) {
            using(var session = _documentStore.QuerySession()) {
                return await session.Query<CollectorConfigStored>()
                    .FirstOrDefaultAsync( x => x.SchedulerAgentId.Equals(schedulerAgentId));
            }
        }
    }
}