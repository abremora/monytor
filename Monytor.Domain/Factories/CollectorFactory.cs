using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors;
using Monytor.Implementation.Collectors.NetFramework;
using System;

namespace Monytor.Domain.Factories {
    public static class CollectorFactory {
        public static Collector CreateCollector(AddCollectorToConfigCommand addCollectorToConfigCommand) {
            switch (addCollectorToConfigCommand) {
                case AddSqlCollectorToConfigCommand command:
                    return SqlCollectorFactory.CreateCollector(command);
                case AddRavenDbCollectorToConfigCommand command:
                    return RavenDbCollectorFactory.CreateCollector(command);
                case AddPerformanceCounterCollectorToConfigCommand command:
                    return new PerformanceCounterCollector() {
                        Category = command.Category,
                        Counter = command.Counter,
                        MachineName = command.MachineName,
                        Instance = command.Instance
                    };
                case AddRestApiCollectorToConfigCommand command:
                    return new RestApiCollector() {
                        JsonPath = command.JsonPath,
                        RequestUri = command.RequestUri != null ? new Uri(command.RequestUri) : null,
                        TagName = command.TagName
                    };
                case AddSystemInformationCollectorToConfigCommand command:
                    return new SystemInformationCollector();
                default:
                    throw new NotSupportedException($"{addCollectorToConfigCommand?.GetType()} is not supported yet.");
            }
        }      
    }
}
