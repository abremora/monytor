using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Models;

namespace Monytor.Core.Services {
    public interface ICollectorConfigService {
        CollectorConfigStored Get(string id);
        string Create(CreateCollectorConfigCommand command);
        void AddCollector(string collectorConfigId, AddCollectorToConfigCommand command);
    }
}