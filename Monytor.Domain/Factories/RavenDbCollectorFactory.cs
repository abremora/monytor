using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Configurations;
using System;

namespace Monytor.Domain.Factories {
    public static class RavenDbCollectorFactory {
        public static Collector CreateCollector(AddRavenDbCollectorToConfigCommand ravenDbCollectorCommand) {
            switch (ravenDbCollectorCommand) {
                case AddRavenDbAllCollectionCollectorToConfigCommand command:
                    return new Implementation.Collectors.RavenDb.AllCollectionCollector() {
                        Source = CreateDatabaseSourceFromCommand(command)
                    };
                case AddRavenDbCollectionCollectorToConfigCommand command:
                    return new Implementation.Collectors.RavenDb.CollectionCollector() {
                        Source = CreateDatabaseSourceFromCommand(command),
                        CollectionName = command.CollectionName
                    };
                case AddRavenDbStartingWithCollectorToConfigCommand command:
                    return new Implementation.Collectors.RavenDb.StartingWithCollector() {
                        Source = CreateDatabaseSourceFromCommand(command),
                        StartingWith = command.StartingWith
                    };
                default:
                    throw new NotSupportedException($"{ravenDbCollectorCommand?.GetType()} is not supported yet.");
            }
        }

        private static DatabaseSource CreateDatabaseSourceFromCommand(AddRavenDbCollectorToConfigCommand ravenDbCollectorCommand) {
            return new DatabaseSource() {
                Database = ravenDbCollectorCommand.DatabaseName,
                Url = ravenDbCollectorCommand.DatabaseUrl
            };
        }
    }
}
