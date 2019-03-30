using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monytor.Core.Configurations;

namespace Monytor.Core.Services {
    public interface ISchedulerCollectorConfigService {
        Task<CollectorConfig> GetCollectorConfigurationAsync(bool forceReload = false);
    }
}
