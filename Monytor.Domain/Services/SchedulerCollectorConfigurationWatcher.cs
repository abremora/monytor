using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Monytor.Core.Configurations;
using Monytor.Core.Services;

namespace Monytor.Domain.Services {
    public class SchedulerCollectorConfigurationWatcher : IDisposable, ISchedulerCollectorConfigurationWatcher {
        private static readonly TimeSpan MinimumPollingInterval = new TimeSpan(0, 0, 15);
        private readonly ISchedulerCollectorConfigurationReadService _schedulerCollectorConfigurationReadService;
        private readonly Subject<bool> _stopTimer = new Subject<bool>();
        

        private SchedulerConfiguration _compareConfiguration;
        private CollectorConfig _compareCollectorConfig;
        
        public Subject<SchedulerConfigurationChangeResult> SchedulerConfigurationChanged { get; } = new Subject<SchedulerConfigurationChangeResult>();

        public SchedulerCollectorConfigurationWatcher(
            ISchedulerCollectorConfigurationReadService schedulerCollectorConfigurationReadService) {
            _schedulerCollectorConfigurationReadService = schedulerCollectorConfigurationReadService;
        }

        public void Dispose() {
            _stopTimer?.OnNext(true);
            _stopTimer?.OnCompleted();
            _stopTimer?.Dispose();
        }

        public void BeginCollectorConfigurationChangePolling(SchedulerConfiguration compareConfiguration,
            CollectorConfig compareCollectorConfig) {
            _compareCollectorConfig = compareCollectorConfig;
            _compareConfiguration = compareConfiguration;
            if (_compareConfiguration.CollectorPollingInterval < MinimumPollingInterval) {
                // Log this away or throw exception
                return;
            }

            Observable.Timer(_compareConfiguration.CollectorPollingInterval,
                _compareConfiguration.CollectorPollingInterval)
                .TakeUntil(_stopTimer)
                .Subscribe( _ => PollConfigurationForChanges());
        }

        private void PollConfigurationForChanges() {
            var loadedConfig =
                _schedulerCollectorConfigurationReadService.LoadSchedulerConfiguration(_compareConfiguration);
            var result = AnalyzeConfigurationChanges(loadedConfig, _compareCollectorConfig);
            if (result.HasChanges) {
                SchedulerConfigurationChanged?.OnNext(result);
            }
        }


        public void StopCollectorConfigurationChangePolling() {
            _stopTimer?.OnNext(true);
        }

        private SchedulerConfigurationChangeResult AnalyzeConfigurationChanges(CollectorConfig loadedConfig, CollectorConfig compareConfiguration) {
            var result = new SchedulerConfigurationChangeResult();

            return result;
        }
    }
}