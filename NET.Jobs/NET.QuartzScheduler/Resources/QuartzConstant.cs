namespace NET.QuartzScheduler.Services.resources
{
    public class QuartzConstant
    {
        public const string DEFAULT_QUARTZ_DATA_SOURCE = "datasource";
        public const string DEFAULT_QUARTZ_FREFIX = "QRTZ_";
        
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
