using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monytor.Core;
using Monytor.Core.Configurations;

namespace Monytor.Domain.Services {
    public static class CollectorConfigChangeAnalyzer {

        public static CollectorConfigChangeResult AnalyzeConfigurationChanges(CollectorConfig newCollectorConfig,
            CollectorConfig currentCollectorConfig) {
            var configurationChangeResult = new CollectorConfigChangeResult();

            // ToDo: This will only work for the stored collectors, because file config collectors does not have an Id yet.
            AnalyzeNotificationChanges(configurationChangeResult, newCollectorConfig.Notifications,
                currentCollectorConfig.Notifications);

            AnalyzeCollectorChanges(configurationChangeResult, newCollectorConfig.Collectors,
                currentCollectorConfig.Collectors);

            return configurationChangeResult;
        }

        private static void AnalyzeNotificationChanges(CollectorConfigChangeResult configurationChangeResult,
            List<Notification> loadedConfigNotifications, List<Notification> compareConfigurationNotifications) {
            var removedNotifications = compareConfigurationNotifications.Where(w =>
                loadedConfigNotifications.All(x => !x.Id.EqualsIgnoreCase(w.Id)));
            configurationChangeResult.RemovedNotifications.AddRange(removedNotifications);

            foreach (var notification in loadedConfigNotifications) {
                if (string.IsNullOrWhiteSpace(notification.Id))
                    continue;

                var compareNotification = compareConfigurationNotifications.FirstOrDefault(f =>
                    f.Id.EqualsIgnoreCase(notification.Id));
                if (compareNotification == null) {
                    configurationChangeResult.AddedNotifications.Add(notification);
                }
                else if (!compareNotification.IsEqualByJsonCompare(notification)) {
                    configurationChangeResult.ChangedNotifications.Add(notification);
                }
            }
        }

        private static void AnalyzeCollectorChanges(CollectorConfigChangeResult configurationChangeResult,
            List<Collector> loadedConfigCollectors, List<Collector> compareConfigurationCollectors) {
           
            var removedCollectors = compareConfigurationCollectors.Where(w =>
                loadedConfigCollectors.All(x => !x.Id.EqualsIgnoreCase(w.Id)));
            configurationChangeResult.RemovedCollectors.AddRange(removedCollectors);

            foreach (var collector in loadedConfigCollectors) {
                if (string.IsNullOrWhiteSpace(collector.Id))
                    continue;

                var compareCollector = compareConfigurationCollectors.FirstOrDefault(f =>
                    f.Id.EqualsIgnoreCase(collector.Id));
                if (compareCollector == null) {
                    configurationChangeResult.AddedCollectors.Add(collector);
                }
                else if (!compareCollector.IsEqualByJsonCompare(collector)) {
                    configurationChangeResult.ChangedCollectors.Add(collector);
                }
            }
        }
    }
}
