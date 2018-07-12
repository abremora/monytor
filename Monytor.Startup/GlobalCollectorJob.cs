using Quartz;
using Raven.Client;
using System.Threading.Tasks;
using Autofac;
using System;
using Monytor.Core.Configurations;
using Monytor.Core.Repositories;
using Monytor.Infrastructure;
using System.Linq;

namespace Monytor.Startup {
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
                    var collectorKey = collectorInstance.GetType();
                    var collectorBehavior = _container.ResolveKeyed<CollectorBehaviorBase>(collectorKey);
                    var series = collectorBehavior.Run(collectorInstance)
                        .ToList();

                    using (var bulk = _store.BulkInsert()) {
                        foreach (var serie in series) {
                            bulk.Store(serie);
                        }
                    }

                    if (collectorInstance.Verifiers != null) {
                        foreach (var serie in series) {
                            foreach (var verifier in collectorInstance.Verifiers) {
                                if (verifier == null 
                                    || verifier.Notifications == null
                                    || verifier.Notifications.Count == 0) continue;

                                var verifierKey = verifier.GetType();
                                var verifierBehavior = _container.ResolveKeyed<VerifierBehaviorBase>(verifierKey);                                
                                verifierBehavior.SeriesRepository = _container.Resolve<ISeriesRepository>();
                                var result = verifierBehavior.Verify(verifier, serie);

                                if (result.Successful) {
                                    foreach (var notificationId in verifier.Notifications) {
                                        if (!_container.IsRegisteredWithName<NotificationBehaviorBase>(notificationId)) {
                                            Logger.Error($"'{collectorKey}/{verifierKey}/{notificationId}' not found.");
                                            continue;
                                        }
                                        var notificationBehavior = _container.ResolveNamed<NotificationBehaviorBase>(notificationId);
                                        var notification = _container.ResolveNamed<Notification>(notificationId);
                                        notificationBehavior.Run(notification, result.NotificationShortDescription, result.NotificationLongDescription);
                                    }
                                }
                            }
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