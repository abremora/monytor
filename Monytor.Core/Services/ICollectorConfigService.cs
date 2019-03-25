using Monytor.Core.Models;

namespace Monytor.Core.Services {
    public interface ICollectorConfigService {
        CollectorConfigStored Get(string id);
        void Create(CollectorConfigStored collectorConfig);
    }
}