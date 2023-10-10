using NET.QuartzScheduler.Bus.Services;
using Quartz;

namespace NET.QuartzScheduler.Bus.Jobs
{
    [DisallowConcurrentExecution]

    public class JobBaseRunner : IJob
    {
        public delegate Task<bool> JobExcuterDelegate();

        private readonly IJobHistoryService _jobHistoryService;
        private JobExcuterDelegate _executer;
        public JobBaseRunner(IJobHistoryService jobHistoryService)
        {
            _jobHistoryService = jobHistoryService;
        }
        public void SetExecuter(JobExcuterDelegate input) {
            _executer = input;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            
        }
    }
}
