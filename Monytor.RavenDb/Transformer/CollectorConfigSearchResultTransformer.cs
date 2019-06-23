using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monytor.Core.Models;
using Monytor.RavenDb.Indices;
using Raven.Client.Indexes;

namespace Monytor.RavenDb.Transformer {
    public class CollectorConfigSearchResultTransformer : AbstractTransformerCreationTask<CollectorConfigStored> {
        public CollectorConfigSearchResultTransformer() {
            TransformResults = results => from result in results
                select new CollectorConfigSearchResult() {
                    CollectorConfigId = result.Id,
                    DisplayName = result.DisplayName,
                    SchedulerAgentId = result.SchedulerAgentId,
                    CollectorCount = result.Collectors.Count,
                    NotificationCount = result.Notifications.Count
                    };
        }
    }
}
