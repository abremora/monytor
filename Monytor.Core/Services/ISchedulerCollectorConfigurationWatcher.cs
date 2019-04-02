using System.Reactive.Subjects;
using Monytor.Core.Configurations;

namespace Monytor.Core.Services {
    public interface ISchedulerCollectorConfigurationWatcher {
        Subject<SchedulerConfigurationChangeResult> SchedulerConfigurationChanged { get; }
        void BeginCollectorConfigurationChangePolling(SchedulerConfiguration compareConfiguration, CollectorConfig compareCollectorConfig);
        void StopCollectorConfigurationChangePolling();
    }
}