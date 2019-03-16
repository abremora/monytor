using System.Threading.Tasks;

namespace Monytor.Sheduler.NetCore {
    class Program : SheduleRunner {
        static async Task Main(string[] args) {
            await new Program().RunAsync(args);
        }        
    }
}