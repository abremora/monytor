using System;
using System.Threading.Tasks;
using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using Monytor.Domain.Factories;

namespace Monytor.Domain.Services {
    public class CollectorConfigService : ICollectorConfigService {
        private readonly ICollectorConfigRepository _collectorConfigRepository;

        public CollectorConfigService(ICollectorConfigRepository collectorConfigRepository) {
            _collectorConfigRepository = collectorConfigRepository;
        }

        public Task<CollectorConfigStored> GetCollectorConfigAsync(string collectorConfigId) {
            var config = _collectorConfigRepository.Get(collectorConfigId);
            return Task.FromResult(config);
        }

        public Task<string> CreateCollectorConfigAsync(CreateCollectorConfigCommand command) {
            var newConfig = new CollectorConfigStored() {
                Id = CollectorConfigStored.CreateId(),
                DisplayName = command.DisplayName,
                SchedulerAgentId = command.SchedulerAgentId
            };
            _collectorConfigRepository.Store(newConfig);
            return Task.FromResult(newConfig.Id);
        }

        public Task DeleteCollectorConfigAsync(string collectorConfigId) {
            _collectorConfigRepository.Delete(collectorConfigId);
            return Task.CompletedTask;
        }

        public Task AddCollectorAsync(string collectorConfigId, AddCollectorToConfigCommand addCollectorCommand) {
            Collector collector = CollectorFactory.CreateCollector(addCollectorCommand);
            if (collector != null) {
                SetCollectorValues(collector, addCollectorCommand);
                AddCollectorToConfig(collectorConfigId, collector);
            }
            return Task.CompletedTask;
        }

        public Task DeleteCollectorAsync(string collectorConfigId, string collectorId) {
            var config = _collectorConfigRepository.Get(collectorConfigId);
            config.Collectors.RemoveAll(collector => collector.Id.Equals(collectorId, StringComparison.InvariantCultureIgnoreCase));
            return Task.CompletedTask; 
        }

        private void AddCollectorToConfig(string collectorConfigId, Collector collector) {
            var config = _collectorConfigRepository.Get(collectorConfigId);
            config.Collectors.Add(collector);            
        }

        private void SetCollectorValues(Collector collector, AddCollectorToConfigCommand command) {
            collector.Id = collector.CreateId();
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
            collector.Verifiers = new System.Collections.Generic.List<Verifier>();
        }

        
    }
}
