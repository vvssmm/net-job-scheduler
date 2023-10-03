using NET.QuartzScheduler.Services.models;
using NET.QuartzScheduler.Services.Models;
using NET.QuartzScheduler.Services.resources;
using Quartz;
using Quartz.Impl.Matchers;
using System.Runtime.InteropServices;
using TimeZoneConverter;

namespace NET.QuartzScheduler.Services.Services
{
    public class QuartzBaseService : IQuartzBaseService
    {
        private readonly ISchedulerFactory _scheduleFactory;

        private string _nzTimeZoneKey = QuartzConstant.HCMC_TIME_ZONE;

        private const string NOTFOUND = "Not found job with key {0}";
        private const string INVALID_CRON = "Cron expression is invalid. {0}";
        private const string JOB_TYPE_IS_NULL = "Invalid JobType";
        public QuartzBaseService(ISchedulerFactory scheFactory)
        {
            _scheduleFactory = scheFactory;
        }

        public async Task<BaseResult> CreateJob(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new BaseResult();
            if (jobSchedule.JobType == null)
            {
                retResult.Messages.Add(JOB_TYPE_IS_NULL);
                return retResult;
            }
            if (jobSchedule.StartNow || jobSchedule.StartTime.HasValue || CronExpression.IsValidExpression(jobSchedule.CronExpression))
            {
                var schedule = await _scheduleFactory.GetScheduler();
                var job = CreateNewJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule,job);
                jobSchedule.StartTime = trigger.StartTimeUtc.DateTime;
                await schedule.ScheduleJob(job, trigger);
                retResult.IsSuccess = true;
            }
            else
            {
                retResult.Messages.Add(string.Format(INVALID_CRON, jobSchedule.CronExpression));
            }
            return retResult;
        }
        public async Task<BaseResult> PauseJob(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new BaseResult();
            var schedule = await _scheduleFactory.GetScheduler();
            var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);
            var existedJob = await schedule.GetJobDetail(jobKey);
            if (existedJob != null)
            {
                await schedule.PauseJob(jobKey);
                retResult.IsSuccess = true;
            }
            else
            {
                retResult.Messages.Add(string.Format(NOTFOUND, jobKey.ToString()));
            }
            return retResult;
        }
        public async Task<BaseResult> ResumeJob(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new BaseResult();
            var schedule = await _scheduleFactory.GetScheduler();

            var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);
            var existedJob = await schedule.GetJobDetail(jobKey);
            if (existedJob != null)
            {
                await schedule.ResumeJob(jobKey);
                retResult.IsSuccess = true;
            }
            else
            {
                retResult.Messages.Add(string.Format(NOTFOUND, jobKey.ToString()));
            }
            return retResult;
        }
        public async Task<BaseResult> DeleteJob(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new BaseResult();
            var schedule = await _scheduleFactory.GetScheduler();

            var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);
            var isJobExsited = await schedule.CheckExists(jobKey);
            if (isJobExsited)
            {
                await schedule.DeleteJob(jobKey);
            }
            retResult.IsSuccess = true;
            return retResult;

        }
        public async Task<BaseResult> TriggerNow(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new BaseResult();
            var schedule = await _scheduleFactory.GetScheduler();

            var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);
            var existedJob = await schedule.CheckExists(jobKey);
            if (existedJob)
            {
                if (jobSchedule.JobData != null && jobSchedule.JobData.Any())
                {
                    var jobDataMap = new JobDataMap();

                    foreach (var item in jobSchedule.JobData)
                    {
                        jobDataMap.Put(item.Key, item.Value);
                    }
                    await schedule.TriggerJob(jobKey, jobDataMap);
                }
                else
                {
                    await schedule.TriggerJob(jobKey);
                }
                retResult.IsSuccess = true;
            }
            else
            {
                jobSchedule.StartNow = true;
                jobSchedule.CronExpression = string.Empty;
                retResult = await CreateJob(jobSchedule);
            }

            return retResult;
        }
        public async Task<BaseResult> UpdateJob(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new BaseResult();
            if (CronExpression.IsValidExpression(jobSchedule.CronExpression))
            {
                var schedule = await _scheduleFactory.GetScheduler();
                var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);
                var existedJob = await schedule.CheckExists(jobKey);
                if (existedJob)
                {
                    var oldTriggerKey = new TriggerKey(jobSchedule.JobName,jobSchedule.JobGroup);
                    var oldTrigger = await schedule.GetTrigger(oldTriggerKey);
                    if (oldTrigger != null)
                    {
                        await schedule.PauseJob(jobKey);
                        var newTrigger = oldTrigger.GetTriggerBuilder()
                                                .WithDescription(jobSchedule.Description)
                                                .WithCronSchedule(jobSchedule.CronExpression,
                                                 x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(_nzTimeZoneKey))).Build();

                        await schedule.RescheduleJob(oldTrigger.Key, newTrigger);
                        await schedule.ResumeJob(jobKey);
                        retResult.IsSuccess = true;
                    }
                }
                else
                {
                    retResult.Messages.Add($"Job {jobKey.ToString()} not found");
                }
            }
            else
            {
                retResult.Messages.Add("Invalid Cron Expression");
            }
            return retResult;
        }
        public async Task<BaseResult> RegisterJob(QuartzSchedulerJobModel jobSchedule, bool isReplaceInfo)
        {
            var retResult = new BaseResult();
            var schedule = await _scheduleFactory.GetScheduler();
            var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);
            var existedJob = await schedule.GetJobDetail(jobKey);
            if (existedJob == null)
            {
                retResult = await CreateJob(jobSchedule);
            }
            else
            {
                if (isReplaceInfo)
                {
                    retResult = await UpdateJob(jobSchedule);
                }
                else
                {
                    retResult.IsSuccess = true;
                }
            }
            return retResult;
        }
        public async Task<ResultModel<IList<TriggerJobModel>>> GetAllJob()
        {
            var retResult = new ResultModel<IList<TriggerJobModel>>()
            {
                Data = new List<TriggerJobModel>()
            };
            var scheduler = await _scheduleFactory.GetScheduler();
            var allTrigger = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            foreach (var triggerKey in allTrigger)
            {
                if ((await scheduler.GetTrigger(triggerKey)) is ICronTrigger trigger)
                {
                    DateTime? previousTime = trigger.GetPreviousFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.UtcDateTime : null;
                    DateTime? nextFireTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.UtcDateTime : null;

                    var jobModel = new TriggerJobModel()
                    {
                        NextFireTime = nextFireTime,
                        PreviousFireTime = previousTime,
                        CronExpression = trigger.CronExpressionString ?? string.Empty,
                        Description = trigger.Description ?? string.Empty,
                        Status = (int)(await scheduler.GetTriggerState(triggerKey)),
                        JobName = trigger.JobKey.Name,
                        JobGroup = trigger.JobKey.Group
                    };
                    retResult.Data.Add(jobModel);
                }
            }
            retResult.IsSuccess = true;
            return retResult;
        }
        public async Task<ResultModel<TriggerJobModel>> GetJob(QuartzSchedulerJobModel jobSchedule)
        {
            var retResult = new ResultModel<TriggerJobModel>();
            var scheduler = await _scheduleFactory.GetScheduler();
            var key = new TriggerKey(jobSchedule.JobName,jobSchedule.JobGroup);

            if (await scheduler.GetTrigger(key) is ICronTrigger trigger)
            {
                DateTime? previousTime = trigger.GetPreviousFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.UtcDateTime : null;
                DateTime? nextFireTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.UtcDateTime : null;

                retResult.Data = new TriggerJobModel()
                {
                    NextFireTime = nextFireTime,
                    PreviousFireTime = previousTime,
                    CronExpression = trigger.CronExpressionString ?? string.Empty,
                    Description = trigger.Description ?? string.Empty,
                    Status = (int)(await scheduler.GetTriggerState(trigger.Key)),
                    JobName = trigger.JobKey.Name,
                    JobGroup = trigger.JobKey.Group,
                };
                retResult.IsSuccess = true;
            }
            else
            {
                retResult.Messages.Add($"Job {new JobKey(jobSchedule.JobName, jobSchedule.JobGroup)} not found");
            }
            return retResult;
        }
        public async Task StartScheduler()
        {
            var scheduler = await _scheduleFactory.GetScheduler();
            if (scheduler.IsStarted == false) await scheduler.Start();
        }
        public async Task ShutdownScheduler()
        {
            var scheduler = await _scheduleFactory.GetScheduler();
            if (scheduler.IsStarted == true) await scheduler.Shutdown();
        }

        #region privateFunction
        private ITrigger CreateTrigger(QuartzSchedulerJobModel schedule, IJobDetail jobDetail)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _nzTimeZoneKey = TZConvert.WindowsToIana(_nzTimeZoneKey);
            }
            var tiggerBuilder = TriggerBuilder
                .Create()
                .ForJob(jobDetail)
                .WithIdentity(new TriggerKey(schedule.JobName,schedule.JobGroup))
                .WithDescription(schedule.Description);

            if (schedule.StartNow)
            {
                tiggerBuilder.StartNow();
            }
            else if (schedule.StartTime.HasValue)
            {
                tiggerBuilder.StartAt(schedule.StartTime.Value.ToUniversalTime());
            }
            else
            {
                tiggerBuilder.WithCronSchedule(schedule.CronExpression, x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(_nzTimeZoneKey)));
            }
            if (schedule.Priority > 0 && schedule.Priority <= 5)
            {
                tiggerBuilder.WithPriority(schedule.Priority);
            }
            return tiggerBuilder.Build();
        }
        private static IJobDetail CreateNewJob(QuartzSchedulerJobModel schedule)
        {
            var jobType = schedule.JobType;
            var jobBulder = JobBuilder
                 .Create(jobType)
                 .WithIdentity(new JobKey(schedule.JobName, schedule.JobGroup))
                 .WithDescription(schedule.Description);
            if (schedule.JobData != null && schedule.JobData.Any())
            {
                foreach (var item in schedule.JobData)
                {
                    jobBulder = jobBulder.UsingJobData(item.Key, item.Value);
                }
            }
            return jobBulder.Build();
        }
        #endregion
    }
}
