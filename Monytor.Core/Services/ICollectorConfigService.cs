using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Models;
using System.Threading.Tasks;

namespace Monytor.Core.Services {
    public interface ICollectorConfigService {
        Task<CollectorConfigStored> GetCollectorConfigAsync(string collectorConfigId);
        Task<string> CreateCollectorConfigAsync(CreateCollectorConfigCommand command);
        Task AddCollectorAsync(string collectorConfigId, AddCollectorToConfigCommand command);
        Task DeleteCollectorConfigAsync(string collectorConfigId);
        Task DeleteCollectorAsync(string collectorConfigId, string collectorId);
    }
}