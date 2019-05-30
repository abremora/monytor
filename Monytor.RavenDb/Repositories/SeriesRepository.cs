using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.RavenDb.Repositories {
    public class SeriesRepository : ISeriesRepository {
        private readonly IUnitOfWork _unitOfWork;

        public SeriesRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public Series GetSeries(int id) {
            return _unitOfWork.Session.Load<Series>(id);
        }

        public void Store(Series series) {
            _unitOfWork.Session.Store(series);
        }
    }
}
