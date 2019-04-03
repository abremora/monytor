using Monytor.Core.Models;
using System;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface IDashboardRepository {
        Dashboard Get(string id);
        IEnumerable<Dashboard> LoadOverview();
        void Store(Dashboard config);
    }
}