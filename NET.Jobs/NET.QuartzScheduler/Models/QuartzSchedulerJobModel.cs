using System.ComponentModel.DataAnnotations;

namespace NET.QuartzScheduler.Services.models
{
    public class QuartzSchedulerJobModel
    {
        public QuartzSchedulerJobModel(Type jobType, string cronExpression, string name, bool startNow, string time = "")
        {
            JobType = jobType;
            CronExpression = cronExpression;
            JobName = name;
            TimeZone = time;
            StartNow = startNow;
        }

        public QuartzSchedulerJobModel() { }
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
        public decimal? JobLogId { get; set; }
        [Required]
        public string JobName { get; set; }
        [Required]
        public string JobGroup { get; set; }
        public string TimeZone { get; set; }
        public bool StartNow { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public int Priority { get; set; }
        public Dictionary<string, string> JobData { get; set; }
    }
    public class TriggerJobModel
    {
        public DateTime? PreviousFireTime { get; set; }
        public DateTime? NextFireTime { get; set; }
        public string TimeZone { get; set; }
        public string Description { get; set; }
        public string CronExpression { get; set; }
        public string SchedulerName { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public int? Status { get; set; }
       
    }
}
