using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monytor.Core.Repositories {
    public interface ICollectorConfigRepository {
        CollectorConfigStored Get(string id);
        Task<Search<CollectorConfigSearchResult>> SearchAsync(string searchTerms, int page, int pageSize);
        void Store(CollectorConfigStored collectorConfig);
        void Delete(string id);
        void Delete(CollectorConfigStored collectorConfig);
    }
}

