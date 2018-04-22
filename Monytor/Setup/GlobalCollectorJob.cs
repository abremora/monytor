using Quartz;
using Raven.Client;
using System.Threading.Tasks;
using Autofac;
using System;
using Monytor.Core.Configurations;

namespace Monytor.Infrastructure {
    [DisallowConcurrentExecution]
    public class GlobalCollectorJob : IJob {
        private readonly IDocumentStore _store;
        private readonly ILifetimeScope _container;

        public GlobalCollectorJob(IDocumentStore store, ILifetimeScope container) {
            _store = store;
            _container = container;
        }
        public async Task Execute(IJobExecutionContext context) {
            Collector collectorInstance = null;
            try {
                collectorInstance = context.JobDetail.JobDataMap["CollectorType"] as Collector;

                var next = context.NextFireTimeUtc?.LocalDateTime;
                var nextTimeSpan = context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.Subtract(DateTimeOffset.UtcNow) : TimeSpan.MinValue;

                Logger.Info($"Job: {collectorInstance.GetType().Name} | Next: {next} ({nextTimeSpan.ToString(@"hh\:mm\:ss")})");

                using (var scope = _container.BeginLifetimeScope()) {
                    var series = collectorInstance.Run();

                    using (var bulk = _store.BulkInsert()) {
                        foreach (var serie in series) {
                            bulk.Store(serie);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex, $"{context.JobDetail.Key} in {collectorInstance.GetType()}");                
            }     
        }
    }
}