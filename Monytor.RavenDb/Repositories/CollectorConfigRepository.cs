using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.RavenDb.Indices;
using Monytor.RavenDb.Transformer;
using System.Threading.Tasks;

namespace Monytor.RavenDb.Repositories {
    public class CollectorConfigRepository : ICollectorConfigRepository {
        private readonly UnitOfWork _unitOfWork;

        public CollectorConfigRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public CollectorConfigStored Get(string id) {
            return _unitOfWork.Session.Load<CollectorConfigStored>(id);
        }

        public async Task<Search<CollectorConfigSearchResult>> SearchAsync(string searchTerms, int page, int pageSize) {
            using (var session = _unitOfWork.Store.OpenAsyncSession()) {
                var query = session.Advanced.AsyncDocumentQuery<CollectorConfigIndex.Result, CollectorConfigIndex>()
                    .Search(x => x.Content, searchTerms)
                    .OrderBy(o => o.DisplayName)
                    .Skip(page - 1 * pageSize)
                    .Take(pageSize)
                    .Statistics(out var stats)
                    .SetResultTransformer<CollectorConfigSearchResultTransformer, CollectorConfigSearchResult>();

                var items = await query.ToListAsync();
                return SearchFactory.CreateSearchResult(items, stats, page, pageSize);
            }
        }

        public void Store(CollectorConfigStored collectorConfig) {
            _unitOfWork.Session.Store(collectorConfig);
        }

        public void Delete(string id) {
            _unitOfWork.Session.Delete(id);
        }

        public void Delete(CollectorConfigStored collectorConfig) {
            _unitOfWork.Session.Delete(collectorConfig);
        }
    }
}