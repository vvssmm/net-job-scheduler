namespace NET.QuartzScheduler.Services.resources
{
    public class QuartzConstant
    {
        public const string DEFAULT_QUARTZ_DATA_SOURCE = "datasource";
        public const string DEFAULT_QUARTZ_FREFIX = "QRTZ_";
        public const string HCMC_TIME_ZONE = "SE Asia Standard Time";
    }
    public class QuartzEnums
    {
        public enum QuartzDBTypeEnum
        {
            POSTGRE,
            ORACLE,
            SQL_SERVER
        }
    }
}
