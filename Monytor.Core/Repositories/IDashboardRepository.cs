using System;
using System.Collections.Generic;
using Monytor.WebApi.Controllers;

namespace Monytor.Core.Repositories {
    public interface IDashboardRepository {
        Dashboard Load(string id);
        IEnumerable<Dashboard> LoadOverview();
        void Save(Dashboard config);
    }
}