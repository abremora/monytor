using AutoFixture;
using FluentAssertions;
using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using Monytor.Domain.Factories;
using Monytor.Implementation.Collectors;
using Monytor.Implementation.Collectors.NetFramework;
using Monytor.Implementation.Collectors.RavenDb;
using Monytor.Implementation.Collectors.Sql.PostgreSql;
using Monytor.RavenDb;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monytor.Domain.Services;
using Xunit;

namespace Monytor.Domain.Tests {

    public class CollectorConfigServiceTests : RavenTestContext {

        [Fact]
        public async Task Create_EmptyConfig_Success() {
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var uow = new UnitOfWork(store);
            var configRepository = new CollectorConfigRepository(uow);

            var collectorConfigService = new CollectorConfigService(configRepository);
            var command = new CreateCollectorConfigCommand() {
                DisplayName = "Test",
                SchedulerAgentId = "default"
            };
            uow.OpenSession();
            var id = await collectorConfigService.CreateCollectorConfigAsync(command);
            uow.SaveChanges();

            using (var session = store.OpenSession()) {
                var loadedConfig = session.Load<CollectorConfigStored>(id);
                loadedConfig.Should().NotBeNull();
                loadedConfig.DisplayName.Should().BeEquivalentTo(command.DisplayName);
            }
        }


        [Fact]
        public void AddCollector_PostgreSqlCountCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddSqlCountCollectorToConfigCommand>()
                .With(w => w.SourceProvider, SqlCollectorSourceProvider.PostgreSql)
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<CountCollector>();
        }

        [Fact]
        public void AddCollector_OracleCountCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddSqlCountCollectorToConfigCommand>()
                .With(w => w.SourceProvider, SqlCollectorSourceProvider.Oracle)
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<Implementation.Collectors.Sql.Oracle.CountCollector>();
        }

        [Fact]
        public void AddCollector_MsSqlCountCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddSqlCountCollectorToConfigCommand>()
                .With(w => w.SourceProvider, SqlCollectorSourceProvider.MsSql)
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<Implementation.Collectors.Sql.MsSql.CountCollector>();
        }

        [Fact]
        public void AddCollector_MySqlCountCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddSqlCountCollectorToConfigCommand>()
                .With(w => w.SourceProvider, SqlCollectorSourceProvider.MySql)
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<Implementation.Collectors.Sql.MySql.CountCollector>();
        }

        [Fact]
        public void AddCollector_RavenDbStartingWithCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddRavenDbStartingWithCollectorToConfigCommand>()
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<StartingWithCollector>();
        }

        [Fact]
        public void AddCollector_RavenDbCollectionCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddRavenDbCollectionCollectorToConfigCommand>()
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<CollectionCollector>();
        }

        [Fact]
        public void AddCollector_RavenDbAllCollectionCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddRavenDbAllCollectionCollectorToConfigCommand>()
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<AllCollectionCollector>();
        }

        [Fact]
        public void AddCollector_RestApiCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddRestApiCollectorToConfigCommand>()
                .With( x => x.RequestUri, "https://github.com/t0ms3n/monytor")
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<RestApiCollector>();
        }

        [Fact]
        public void AddCollector_SystemInformationCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddSystemInformationCollectorToConfigCommand>()
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<SystemInformationCollector>();
        }

        [Fact]
        public void AddCollector_PerformanceCounterCollector_CollectorIsInCollectionOfConfig() {
            // Arrange
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var config = new CollectorConfigStored();
            var configRepository = Substitute.For<ICollectorConfigRepository>();
            configRepository.Get(Arg.Any<string>()).Returns(config);

            // Act
            var collectorCommand = new Fixture().Build<AddPerformanceCounterCollectorToConfigCommand>()
                .Create();
            var collectorConfigService = new CollectorConfigService(configRepository);
            collectorConfigService.AddCollectorAsync(config.Id, collectorCommand);

            // Assert
            config.Collectors.Should().AllBeOfType<PerformanceCounterCollector>();
        }


    }
}
