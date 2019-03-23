using Monytor.Implementation.Collectors.NetFramework;
using System.Threading.Tasks;

namespace Monytor.Scheduler.NetFramework {
    class Program : ScheduleRunner {
        static async Task Main(string[] args) {
            await new Program().RunAsync(args);
        }
        
        protected override void SetupBinder() {
            base.SetupBinder();
            new PerformanceCounterCollector();
        }
    }
}
