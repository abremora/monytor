using System;
using System.Threading.Tasks;
using Monytor.Core.Configurations;
using Monytor.Core.Repositories;
using Monytor.Core.Services;

namespace Monytor.Domain.Services {
    public class SchedulerCollectorConfigurationReadService : ISchedulerCollectorConfigurationReadService {
        private readonly ICollectorConfigQueryRepository _collectorConfigQueryRepository;

        public SchedulerCollectorConfigurationReadService(ICollectorConfigQueryRepository collectorConfigQueryRepository) {
            _collectorConfigQueryRepository = collectorConfigQueryRepository;
        }

        public Task<CollectorConfig> LoadSchedulerConfigurationAsync(SchedulerConfiguration schedulerConfiguration)
        {
            switch (schedulerConfiguration?.CollectorConfigProvider)
            {
                case CollectorConfigProvider.File:
                    return Task.FromResult(LoadCollectorConfigFromFile(schedulerConfiguration.CollectorConfigFileName));
                case CollectorConfigProvider.Database:
                    return LoadCollectorConfigFromDatabaseAsync(schedulerConfiguration.SchedulerAgentId);
                default:
                    throw new NotImplementedException();
            }
        }

        public CollectorConfig LoadSchedulerConfiguration(SchedulerConfiguration schedulerConfiguration)
        {
            switch (schedulerConfiguration?.CollectorConfigProvider)
            {
                case CollectorConfigProvider.File:
                    return LoadCollectorConfigFromFile(schedulerConfiguration.CollectorConfigFileName);
                case CollectorConfigProvider.Database:
                    return LoadCollectorConfigFromDatabase(schedulerConfiguration.SchedulerAgentId);
                default:
                    throw new NotImplementedException();
            }
        }


        private CollectorConfig LoadCollectorConfigFromFile(string collectorConfigFileName)
        {
            var configCreator = new CollectorFileConfigCreator(collectorConfigFileName);
            return configCreator.LoadConfig();
        }

        private async Task<CollectorConfig> LoadCollectorConfigFromDatabaseAsync(string schedulerAgentId)
        {
            var collectorConfig = await _collectorConfigQueryRepository.GetByAgentIdAsync(schedulerAgentId);
            if (collectorConfig == null)
            {
                throw new ArgumentException($"The collector configuration for agent {schedulerAgentId} was not found.");
            }
            return collectorConfig;
        }

        private CollectorConfig LoadCollectorConfigFromDatabase(string schedulerAgentId)
        {
            var collectorConfig = _collectorConfigQueryRepository.GetByAgentId(schedulerAgentId);
            if (collectorConfig == null)
            {
                throw new ArgumentException($"The collector configuration for agent {schedulerAgentId} was not found.");
            }
            return collectorConfig;
        }
    }
}