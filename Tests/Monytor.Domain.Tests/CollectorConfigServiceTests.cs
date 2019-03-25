using FluentAssertions;
using Monytor.Core.Models;
using Monytor.Core.Services;
using Monytor.Domain.Services;
using Monytor.PostgreSQL;
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
           var store = base.NewDocumentStore(configureStore: ConfigureTestStore);
            var configRepository = new CollectorConfigRepository(store);


            ICollectorConfigService collectorConfigService = new CollectorConfigService(configRepository);
            var collectorConfig = new CollectorConfigStored() {
                Id = CollectorConfigStored.CreateId()
            };
            collectorConfigService.Create(collectorConfig);

            var loadedConfig = collectorConfigService.Get(collectorConfig.Id);
            loadedConfig.Should().NotBeNull();
        }

       
    }
}
