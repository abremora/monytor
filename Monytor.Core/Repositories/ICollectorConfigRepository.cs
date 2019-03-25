using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monytor.Core.Repositories {
    public interface ICollectorConfigRepository {
        CollectorConfigStored Get(string id);
        void Store(CollectorConfigStored collectorConfig);
    }
}

