using Monytor.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monytor.Core.Configurations;

namespace Monytor.Domain.Services {
    public class SchedulerCollectorConfigService : ISchedulerCollectorConfigService {
        private readonly SchedulerConfiguration _schedulerConfiguration;
        private CollectorConfig _currentCollectorConfig;

        public SchedulerCollectorConfigService(SchedulerConfiguration schedulerConfiguration) {
            _schedulerConfiguration = schedulerConfiguration;
        }

        public Task<CollectorConfig> GetCollectorConfigurationAsync() {
            if(_currentCollectorConfig == null) {
                switch(_schedulerConfiguration.CollectorConfigProvider) {
                    case CollectorConfigProvider.File:
                        _currentCollectorConfig = LoadCollectorConfigFromFile();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return Task.FromResult(_currentCollectorConfig);
        }

        private CollectorConfig LoadCollectorConfigFromFile() {
            var configCreator = new CollectorConfigCreator(_schedulerConfiguration.CollectorConfigFileName);
            return configCreator.LoadConfig();
        }
    }
}