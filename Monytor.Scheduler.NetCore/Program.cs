using System.Threading.Tasks;

namespace Monytor.Scheduler.NetCore {
    class Program : ScheduleRunner {
        static async Task Main(string[] args) {
            await new Program().RunAsync(args);
        }
    }
}