using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monytor.Core.Configurations;
using System.Linq;
using Microsoft.Extensions.Logging;
using Monytor.Core.Services;

namespace Monytor.Startup {
    public class SchedulerStartup : IDisposable {
        private bool _disposedValue = false;
        private readonly IDisposable _schedulerConfigurationChangedSubscription;
        private readonly IScheduler _scheduler;
        private readonly AutofacJobFactory _factory;
        private readonly ILogger<SchedulerStartup> _logger;
        private readonly ISchedulerCollectorConfigurationService _schedulerCollectorConfigurationService;

        public SchedulerStartup(IScheduler scheduler, AutofacJobFactory factory, ILogger<SchedulerStartup> logger,
            ISchedulerCollectorConfigurationService schedulerCollectorConfigurationService,
            ISchedulerCollectorConfigurationWatcher schedulerCollectorConfigurationWatcher) {
            _scheduler = scheduler;
            _factory = factory;
            _logger = logger;
            _schedulerCollectorConfigurationService = schedulerCollectorConfigurationService;
            _schedulerConfigurationChangedSubscription =
                schedulerCollectorConfigurationWatcher.SchedulerConfigurationChanged.Subscribe( 
                    async x => await OnSchedulerConfigurationChanged(x));
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    _schedulerConfigurationChangedSubscription?.Dispose();
                    var task = _scheduler.Shutdown(true);
                    task.Wait(5000);
                }

                _disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        
        public async Task ConfigScheduler() {
            var collectorConfig = await _schedulerCollectorConfigurationService.GetCollectorConfigurationAsync();

            var collectorGroups = collectorConfig.Collectors
                .GroupBy(x => x.GetType());

            foreach (var collectorGroup in collectorGroups) {
                var counter = 0;
                foreach (var collector in collectorGroup) {
                    var identityName = string.IsNullOrEmpty(collector.Id) ?  $"{collector.GetType().Name}({counter++})" : collector.Id;
                    _logger.LogInformation("Register: " + identityName);

                    var job = CreateCollectorJobDetail(collector, identityName);
                    var trigger = CreateCollectorTrigger(collector, identityName);

                    await _scheduler.ScheduleJob(job, trigger);
                    _scheduler.JobFactory = _factory;

                    await _scheduler.Start();
                }
            }
        }

        private static ITrigger CreateCollectorTrigger( Collector collector, string identityName) {
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(identityName, "group")
                .WithPriority(collector.Priority)
                .EndAt(collector.EndAt);

            if (collector.StartingTime.HasValue || collector.RandomTimeDelay.Ticks > 0) {
                var startTime = DateTimeOffset.UtcNow;
                if (collector.StartingTime.HasValue) {
                    startTime = collector.StartingTime.Value;
                }

                startTime = startTime.Add(collector.RandomTimeDelay);
                triggerBuilder.StartAt(startTime);
            }
            else {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.WithSimpleSchedule(x => x
                    .WithInterval(collector.PollingInterval)
                    .RepeatForever())
                .Build();
            return trigger;
        }

        private static IJobDetail CreateCollectorJobDetail(Collector collector, string identityName) {
            var dic = new Dictionary<string, object> {
                {"CollectorType", collector}
            };

            IJobDetail job = JobBuilder.Create<GlobalCollectorJob>()
                .WithIdentity(identityName, "CollectorGroup")
                .SetJobData(new JobDataMap(dic as IDictionary<string, object>))
                .Build();
            return job;
        }

        private async Task OnSchedulerConfigurationChanged(CollectorConfigChangeResult collectorConfigChangeResult) {
            _schedulerCollectorConfigurationService.GetCollectorConfiguration(true);

            foreach (var collector in collectorConfigChangeResult.RemovedCollectors) {
                await _scheduler.DeleteJob(new JobKey(collector.Id, "CollectorGroup"));
                _logger.LogInformation($"Removed collector {collector.Id} from the scheduler due to changed configuration.");
            }

            foreach (var collector in collectorConfigChangeResult.ChangedCollectors) {
                await _scheduler.DeleteJob(new JobKey(collector.Id, "CollectorGroup"));
                var job = CreateCollectorJobDetail(collector, collector.Id);
                var trigger = CreateCollectorTrigger(collector, collector.Id);
                await _scheduler.ScheduleJob(job, trigger);
                _logger.LogInformation($"Updated collector {collector.Id} within the scheduler due to changed configuration.");
            }

            foreach (var collector in collectorConfigChangeResult.AddedCollectors) {
                var job = CreateCollectorJobDetail(collector, collector.Id);
                var trigger = CreateCollectorTrigger(collector, collector.Id);
                await _scheduler.ScheduleJob(job, trigger);
                _logger.LogInformation($"Added collector {collector.Id} to the scheduler due to changed configuration.");
            }
        }
    }
}