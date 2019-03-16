using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors {

    public class SystemInformationCollector : Collector {
        public override string GroupName { get; set; }

        public SystemInformationCollector() {
            GroupName = "SystemInformation";
        }
    }
}
