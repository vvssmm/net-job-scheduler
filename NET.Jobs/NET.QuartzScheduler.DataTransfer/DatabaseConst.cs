namespace NET.QuartzScheduler.DataTransfer
{
    public static class DatabaseConst
    {
        public const string QuartzSchema = "QRTZ";

        public static class Table
        {
            public static class BaseCols
            {
                public const string Id = "ID";
                public const string CreatedDate = "CREATED_DATE";
                public const string CreatedBy = "CREATED_BY";
                public const string UpdatedDate = "UPDATED_DATE";
                public const string UpdatedBy = "UPDATED_BY";
            }
            public static class Jobs
            {
                public const string TableName = "JOBS";
                public static class Cols
                {
                    public const string Name = "NAME";
                    public const string JobClass = "JOB_CLASS";
                    public const string Description = "DESCRIPTION";
                }
            }
            public static class Scheduler
            {
                public const string TableName = "SCHEDULER";
                public static class Cols
                {
                    public const string Name = "NAME";
                    public const string CronExpression = "CRON_EXPRESSION";
                    public const string Description = "DESCRIPTION";
                }
            }

            public static class JobScheduler
            {
                public const string TableName = "JOB_SCHEDULER";
                public static class Cols
                {
                    public const string JobId = "JOB_ID";
                    public const string ScheduleId = "SCHEDULE_ID";
                    public const string Name = "NAME";
                    public const string Description = "DESCRIPTION";
                }
            }
            public static class JobHistory
            {
                public const string TableName = "JOB_HISTORY";
                public static class Cols
                {
                    public const string JobScheduleId = "JOB_SCHEDULE_ID";
                    public const string Status = "STATUS";
                    public const string Description = "DESCRIPTION";
                    public const string StartRunningDateTime = "START_RUNNING_DATETIME";
                    public const string EndRunningTime = "END_RUNNING_DATETIME";
                }
            }
        }
    }
}
