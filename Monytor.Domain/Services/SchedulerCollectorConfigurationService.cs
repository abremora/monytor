using Monytor.Core.Services;
using System.Net;
using System.Threading.Tasks;
using Monytor.Core.Configurations;

namespace Monytor.Domain.Services {
    public class SchedulerCollectorConfigurationService : ISchedulerCollectorConfigurationService {
        private readonly SchedulerConfiguration _schedulerConfiguration;
        private readonly ISchedulerCollectorConfigurationReadService _collectorConfigurationReadService;
        private readonly ISchedulerCollectorConfigurationWatcher _configurationWatcher;
        private CollectorConfig _currentCollectorConfig;

        public SchedulerCollectorConfigurationService(SchedulerConfiguration schedulerConfiguration,
            ISchedulerCollectorConfigurationReadService collectorConfigurationReadService,
            ISchedulerCollectorConfigurationWatcher configurationWatcher
        ) {
            _schedulerConfiguration = schedulerConfiguration;
            _collectorConfigurationReadService = collectorConfigurationReadService;
            _configurationWatcher = configurationWatcher;
        }

        public async Task<CollectorConfig> GetCollectorConfigurationAsync(bool forceReload = false) {
            if (_currentCollectorConfig == null || forceReload) {
                _currentCollectorConfig =
                    await _collectorConfigurationReadService.LoadSchedulerConfigurationAsync(_schedulerConfiguration);
                _configurationWatcher.BeginCollectorConfigurationChangePolling(_schedulerConfiguration, _currentCollectorConfig);
            }

            return _currentCollectorConfig;
        }

        public CollectorConfig GetCollectorConfiguration(bool forceReload = false) {
            if (_currentCollectorConfig == null || forceReload) {
                _currentCollectorConfig =
                    _collectorConfigurationReadService.LoadSchedulerConfiguration(_schedulerConfiguration);
                _configurationWatcher.BeginCollectorConfigurationChangePolling(_schedulerConfiguration, _currentCollectorConfig);
            }
            return _currentCollectorConfig;
        }

    }
}