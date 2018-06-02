using Monytor.Infrastructure;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monytor.Core.Configurations;
using System.Linq;

namespace Monytor.Startup {
    public class SchedulerStartup : IDisposable {
        private readonly IScheduler _scheduler;
        private readonly AutofacJobFactory _factory;

        public SchedulerStartup(IScheduler scheduler, AutofacJobFactory factory) {
            _scheduler = scheduler;
            _factory = factory;
        }

        public async Task ConfigScheduler(CollectorConfig collectorConfig) {
            var collectorGroups = collectorConfig.Collectors
                .GroupBy(x => x.GetType());

            foreach (var collectorGroup in collectorGroups) {
                var counter = 0;
                foreach (var collector in collectorGroup) {
                    var identityName = $"{collector.GetType().Name}({counter})";
                    counter++;
                    Logger.Info("Register: " + identityName);

                    var dic = new Dictionary<string, object> {
                    { "CollectorType", collector }};

                    IJobDetail job = JobBuilder.Create<GlobalCollectorJob>()
                      .WithIdentity(identityName, "CollectorGroup")
                      .SetJobData(new JobDataMap(dic as IDictionary<string, object>))
                      .Build();

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

                    await _scheduler.ScheduleJob(job, trigger);
                    _scheduler.JobFactory = _factory;

                    await _scheduler.Start();
                }
            }
        }

        public void Dispose() {
            _scheduler.Shutdown();
        }
    }
}
