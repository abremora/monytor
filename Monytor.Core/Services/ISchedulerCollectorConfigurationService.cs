using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monytor.Core.Configurations;

namespace Monytor.Core.Services {
    public interface ISchedulerCollectorConfigurationService {
        Task<CollectorConfig> GetCollectorConfigurationAsync(bool forceReload = false);
        CollectorConfig GetCollectorConfiguration(bool forceReload = false);
    }
}
