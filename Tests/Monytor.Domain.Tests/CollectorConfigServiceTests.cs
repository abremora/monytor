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
            var configRepository = new CollectorConfigRepository(store);


            ICollectorConfigService collectorConfigService = new CollectorConfigService(configRepository);
            var command = new CreateCollectorConfigCommand() {
                DisplayName = "Test"
            };
            var id = collectorConfigService.Create(command);

            var loadedConfig = collectorConfigService.Get(id);
            loadedConfig.Should().NotBeNull();
            loadedConfig.DisplayName.Should().BeEquivalentTo(command.DisplayName);
        }       
    }
}
