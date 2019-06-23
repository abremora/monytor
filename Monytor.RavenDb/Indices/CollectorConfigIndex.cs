using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monytor.Core.Models;
using Raven.Client.Indexes;

namespace Monytor.RavenDb.Indices {
    public class CollectorConfigIndex : AbstractIndexCreationTask<CollectorConfigStored, CollectorConfigIndex.Result> {
        public class Result {
            public string DisplayName { get; set; }
            public string SchedulerAgentId { get; set; }
            public object[] Content { get; internal set; }
        }

        public CollectorConfigIndex() {
            Map = docs => from doc in docs
                select new Result {
                    DisplayName = doc.DisplayName,
                    SchedulerAgentId = doc.SchedulerAgentId,
                    Content = new [] {
                        doc.DisplayName,
                        doc.SchedulerAgentId
                    }
                };
        }
    }
}