using System;
using System.Collections.Generic;
using Monytor.Core.Models;
using Monytor.WebApi.Controllers;

namespace Monytor.Core.Services {
    public interface IViewCollectionService {
        ViewCollection Get(string id);
        IEnumerable<ViewCollection> GetOverview();
        void Set(ViewCollection config);
    }
}