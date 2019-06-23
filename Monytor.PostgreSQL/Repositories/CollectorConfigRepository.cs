using System.Linq;
using System.Threading.Tasks;
using Marten;
using Marten.Pagination;
using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL.Repositories {
    public class CollectorConfigRepository : ICollectorConfigRepository {
        private readonly UnitOfWork _unitOfWork;

        public CollectorConfigRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public CollectorConfigStored Get(string id) {
            return _unitOfWork.Session.Load<CollectorConfigStored>(id);
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

        public async Task<Search<CollectorConfigSearchResult>> SearchAsync(string searchTerms, int page, int pageSize) {
            using (var session = _unitOfWork.Store.QuerySession())
            {
                var pagedResult = await session.Query<CollectorConfigStored>()
                    .Where(x => x.Search(searchTerms))
                    .Select(s => new CollectorConfigSearchResult()
                    {
                        CollectorConfigId = s.Id,
                        CollectorCount = s.Collectors.Count,
                        DisplayName = s.DisplayName,
                        NotificationCount = s.Notifications.Count,
                        SchedulerAgentId = s.SchedulerAgentId
                    })
                   .ToPagedListAsync(page, pageSize);
                return SearchFactory.CreateSearchResult(pagedResult);
            }
        }
    }
}