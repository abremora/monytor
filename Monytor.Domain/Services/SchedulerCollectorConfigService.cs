using Monytor.Core.Services;
using System;
using System.Threading.Tasks;
using Monytor.Core.Configurations;
using Monytor.Core.Repositories;

namespace Monytor.Domain.Services {
    public class SchedulerCollectorConfigService : ISchedulerCollectorConfigService {
        private readonly SchedulerConfiguration _schedulerConfiguration;
        private readonly ICollectorConfigQueryRepository _collectorConfigQueryRepository;
        private CollectorConfig _currentCollectorConfig;

        public SchedulerCollectorConfigService(SchedulerConfiguration schedulerConfiguration,
            ICollectorConfigQueryRepository collectorConfigQueryRepository) {
            _schedulerConfiguration = schedulerConfiguration;
            _collectorConfigQueryRepository = collectorConfigQueryRepository;
        }

        public async Task<CollectorConfig> GetCollectorConfigurationAsync(bool forceReload = false) {
            if (_currentCollectorConfig == null || forceReload) {
                switch (_schedulerConfiguration.CollectorConfigProvider) {
                    case CollectorConfigProvider.File:
                        _currentCollectorConfig = LoadCollectorConfigFromFile();
                        break;
                    case CollectorConfigProvider.Database:
                        _currentCollectorConfig = await LoadCollectorConfigFromDatabaseAsync();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return _currentCollectorConfig;
        }

        private CollectorConfig LoadCollectorConfigFromFile() {
            var configCreator = new CollectorFileConfigCreator(_schedulerConfiguration.CollectorConfigFileName);
            return configCreator.LoadConfig();
        }

        private async Task<CollectorConfig> LoadCollectorConfigFromDatabaseAsync() {
            var collectorConfig =  await _collectorConfigQueryRepository.GetByAgentIdAsync(_schedulerConfiguration.SchedulerAgentId);
            if (collectorConfig == null) {
                throw new ArgumentException($"The collector configuration for agent {_schedulerConfiguration.SchedulerAgentId} was not found.");
            }
            return collectorConfig;
        }
    }
}