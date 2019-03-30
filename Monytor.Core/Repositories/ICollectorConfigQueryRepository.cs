using System.Threading.Tasks;
using Monytor.Core.Models;

namespace Monytor.Core.Repositories {
    public interface ICollectorConfigQueryRepository {
        CollectorConfigStored Get(string id);
        Task<CollectorConfigStored> GetAsync(string id);
        CollectorConfigStored GetByAgentId(string schedulerAgentId);
        Task<CollectorConfigStored> GetByAgentIdAsync(string schedulerAgentId);
    }
}