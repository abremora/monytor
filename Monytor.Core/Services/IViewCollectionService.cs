using System;
using System.Collections.Generic;
using Monytor.Core.Models;
using Monytor.WebApi.Controllers;

namespace Monytor.Core.Services {
    public interface IViewCollectionService {
        Dashboard Get(string id);
        IEnumerable<Dashboard> GetOverview();
        void Set(Dashboard config);
    }
}