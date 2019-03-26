using FluentAssertions;
using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Models;
using Monytor.Core.Services;
using Monytor.Domain.Factories;
using Monytor.RavenDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Monytor.Domain.Tests {

    public class CollectorConfigServiceTests : RavenTestContext {

        [Fact]
        public void Create_EmptyConfig_Success() {
            var store = NewDocumentStore(configureStore: ConfigureTestStore);
            var uow = new UnitOfWork(store);
            var configRepository = new CollectorConfigRepository(uow);

            ICollectorConfigService collectorConfigService = new CollectorConfigService(configRepository);
            var command = new CreateCollectorConfigCommand() {
                DisplayName = "Test"
            };
            uow.OpenSession();
            var id = collectorConfigService.Create(command);
            uow.SaveChanges();

            using (var session = store.OpenSession()) {
                var loadedConfig = session.Load<CollectorConfigStored>(id);
                loadedConfig.Should().NotBeNull();
                loadedConfig.DisplayName.Should().BeEquivalentTo(command.DisplayName);
            }
        }
    }
}
