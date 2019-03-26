using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using System;

namespace Monytor.Domain.Factories {
    public class CollectorConfigService : ICollectorConfigService {
        

        private readonly ICollectorConfigRepository _collectorConfigRespository;

        public CollectorConfigService(ICollectorConfigRepository collectorConfigRespository) {
            _collectorConfigRespository = collectorConfigRespository;
        }

        public CollectorConfigStored Get(string id) {
            return _collectorConfigRespository.Get(id);
        }

        public string Create(CreateCollectorConfigCommand command) {
            var newConfig = new CollectorConfigStored() {
                Id = CollectorConfigStored.CreateId(),
                DisplayName = command.DisplayName,
                SchedulerAgentId = command.SchedulerAgentId
            };
            _collectorConfigRespository.Store(newConfig);
            return newConfig.Id;
        }

        public void AddCollector(string collectorConfigId, AddSqlCountCollectorToConfigCommand command) {
            var config = _collectorConfigRespository.Get(collectorConfigId);

            var collector = SqlCollectorFactory.CreateCountCollector(command.SourceProvider);
            collector.DisplayName = command.DisplayName;
            collector.Description = command.Description;
            collector.StartingTime = command.StartingTime.TryParseDateTimeOffsetFromString();
            collector.EndAt = command.EndAt.TryParseDateTimeOffsetFromString();
            collector.Priority = command.Priority;
            collector.OverlappingReccuring = command.OverlappingReccuring;
            collector.GroupName = command.GroupName;
            collector.RandomTimeDelay = command.RandomTimeDelay.TryParseTimeSpanFromString();
            collector.StartingTimeDelay = command.StartingTimeDelay.TryParseTimeSpanFromString();
            collector.PollingInterval = command.PollingInterval.TryParseTimeSpanFromString();
            collector.Verifiers = new System.Collections.Generic.List<Core.Configurations.Verifier>();

            config.Collectors.Add(collector);

            
        }               
    }
}
