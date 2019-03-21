using Monytor.Core.Models;
using System;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface IDashboardRepository {
        Dashboard Load(string id);
        IEnumerable<Dashboard> LoadOverview();
        void Save(Dashboard config);
    }
}