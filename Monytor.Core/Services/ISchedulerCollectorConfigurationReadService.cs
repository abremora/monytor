using System.Threading.Tasks;
using Monytor.Core.Configurations;

namespace Monytor.Core.Services {
    public interface ISchedulerCollectorConfigurationReadService {
        Task<CollectorConfig> LoadSchedulerConfigurationAsync(SchedulerConfiguration schedulerConfiguration);
        CollectorConfig LoadSchedulerConfiguration(SchedulerConfiguration schedulerConfiguration);
    }
}