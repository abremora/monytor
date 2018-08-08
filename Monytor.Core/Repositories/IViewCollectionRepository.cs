using System;
using System.Collections.Generic;
using Monytor.WebApi.Controllers;

namespace Monytor.Domain.Services {
    public interface IViewCollectionRepository {
        ViewCollection Load(string id);
        IEnumerable<ViewCollection> LoadOverview();
        void Save(ViewCollection config);
    }
}