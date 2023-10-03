using NET.QuartzScheduler.Services.models;
using NET.QuartzScheduler.Services.Models;

namespace NET.QuartzScheduler.Services.Services
{
    public interface IQuartzBaseService
    {
        Task<BaseResult> CreateJob(QuartzSchedulerJobModel jobSchedule);
        Task<BaseResult> PauseJob(QuartzSchedulerJobModel jobSchedule);
        Task<BaseResult> ResumeJob(QuartzSchedulerJobModel jobSchedule);
        Task<BaseResult> DeleteJob(QuartzSchedulerJobModel jobSchedule);
        Task<BaseResult> TriggerNow(QuartzSchedulerJobModel jobSchedule);
        Task<BaseResult> UpdateJob(QuartzSchedulerJobModel jobSchedule);
        Task<BaseResult> RegisterJob(QuartzSchedulerJobModel jobSchedule, bool isReplaceInfo);
        Task<ResultModel<IList<TriggerJobModel>>> GetAllJob();
        Task<ResultModel<TriggerJobModel>> GetJob(QuartzSchedulerJobModel jobSchedule);
        Task StartScheduler();
        Task ShutdownScheduler();
    }
}
