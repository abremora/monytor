using Monytor.Implementation.Collectors.NetFramework;
using System.Threading.Tasks;

namespace Monytor.Sheduler.NetFramework {
    class Program : SheduleRunner {
        static async Task Main(string[] args) {
            await new Program().RunAsync(args);
        }
        
        protected override void SetupBinder() {
            base.SetupBinder();
            new PerformanceCounterCollector();
        }
    }
}
