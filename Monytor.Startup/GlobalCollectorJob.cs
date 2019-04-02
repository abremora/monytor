using Quartz;
using System.Threading.Tasks;
using Autofac;
using System;
using Monytor.Core.Configurations;
using Monytor.Core.Repositories;
using System.Linq;
using Microsoft.Extensions.Logging;
using Monytor.Core.Services;

namespace Monytor.Startup {
    [DisallowConcurrentExecution]
    public class GlobalCollectorJob : IJob {
        private readonly IBulkRepository _bulkRepository;
        private readonly ILifetimeScope _container;
        private readonly ILogger<GlobalCollectorJob> _logger;
        private readonly ISchedulerCollectorConfigurationService _schedulerCollectorConfigurationService;

        public GlobalCollectorJob(IBulkRepository bulkRepository, ILifetimeScope container, ILogger<GlobalCollectorJob> logger, ISchedulerCollectorConfigurationService schedulerCollectorConfigurationService) {
            _bulkRepository = bulkRepository;
            _container = container;
            _logger = logger;
            _schedulerCollectorConfigurationService = schedulerCollectorConfigurationService;
        }

        public Task Execute(IJobExecutionContext context) {
            if (context.CancellationToken.IsCancellationRequested)
                return Task.FromCanceled(context.CancellationToken);

            Collector collectorInstance = null;
            try {
                collectorInstance = context.JobDetail.JobDataMap["CollectorType"] as Collector;

                var next = context.NextFireTimeUtc?.LocalDateTime;
                var nextTimeSpan = context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.Subtract(DateTimeOffset.UtcNow) : TimeSpan.MinValue;

                _logger.LogInformation($"Job: {collectorInstance.GetType().Name} | Group:{collectorInstance.GroupName} | Text:'{collectorInstance.DisplayName}' | Next: {next} ({nextTimeSpan.ToString(@"hh\:mm\:ss")})");
                
                using (var scope = _container.BeginLifetimeScope()) {
                    var collectorKey = collectorInstance.GetType().FullName;
                    var collectorBehavior = _container.ResolveKeyed<CollectorBehaviorBase>(collectorKey);
                    var series = collectorBehavior.Run(collectorInstance)
                        .ToList();

                    using (var bulk = _bulkRepository.BeginBulkInsert()) {
                        foreach (var serie in series) {
                            _bulkRepository.Store(serie);
                        }
                    }

                    if (collectorInstance.Verifiers != null) {
                        foreach (var serie in series) {                            
                            foreach (var verifier in collectorInstance.Verifiers) {
                                if (verifier?.Notifications == null || verifier.Notifications.Count == 0) continue;

                                var verifierKey = verifier.GetType().FullName;
                                var verifierBehavior = _container.ResolveKeyed<VerifierBehaviorBase>(verifierKey);                                
                                verifierBehavior.SeriesRepository = _container.Resolve<ISeriesQueryRepository>();
                                var result = verifierBehavior.Verify(verifier, serie);

                                if (result.Successful) {
                                    var currentConfiguration = _schedulerCollectorConfigurationService.GetCollectorConfiguration();

                                    foreach (var notificationId in verifier.Notifications) {
                                        var notification = currentConfiguration.Notifications.FirstOrDefault(f =>
                                            f.Id.Equals(notificationId, StringComparison.InvariantCultureIgnoreCase));
                                        if (notification == null)
                                        {
                                            _logger.LogError($"'No notification with id: '{notificationId}' found.");
                                            continue;
                                        }

                                        var notificationBaseKey = notification.GetType().FullName;
                                        if (!_container.IsRegisteredWithName<NotificationBehaviorBase>(notificationBaseKey)) {
                                            _logger.LogError($"'{notificationBaseKey}' not found.");
                                            continue;
                                        }
                                        var notificationBehavior = _container.ResolveNamed<NotificationBehaviorBase>(notificationBaseKey);
                                        
                                        notificationBehavior.Run(notification, result.NotificationShortDescription, result.NotificationLongDescription);
                                    }
                                }
                            }
                        }
                    }
                }
                return Task.CompletedTask;
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"{context.JobDetail.Key} in {collectorInstance?.GetType()}");
                return Task.FromException(ex);
            }     
            
        }
    }
}