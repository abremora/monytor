using Autofac;
using Quartz;
using Quartz.Spi;

namespace Monytor.Startup {
    public class AutofacJobFactory : IJobFactory {
        private readonly ILifetimeScope _container;

        public AutofacJobFactory(ILifetimeScope container) {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
            return (IJob)_container.Resolve(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job) {            
        }
    }
}
