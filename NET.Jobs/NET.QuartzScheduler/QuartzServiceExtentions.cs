using Microsoft.Extensions.DependencyInjection;
using NET.QuartzScheduler.Services.resources;
using NET.QuartzScheduler.Services.Services;
using Quartz;
using Quartz.AspNetCore;
using static NET.QuartzScheduler.Services.resources.QuartzEnums;

namespace NET.QuartzScheduler
{
    public static class QuartzServiceExtentions
    {
        public static IServiceCollection InstallQuartz(this IServiceCollection services,
               string connectionString,
               QuartzDBTypeEnum dbType,
               string baseSchema,
               string quartPrefix = "",
               string quartzDataSource = "",
               string scheduleId = "",
               string scheduleName = ""
               )
        {
            var props = GetProperty(dbType, connectionString, baseSchema, quartPrefix, quartzDataSource, scheduleId, scheduleName);
            if (string.IsNullOrEmpty(connectionString) == false)
            {
                services.AddQuartz(p =>
                {
                    props.Invoke(p);
                    p.UseTimeZoneConverter();
                });
            }
            services.AddScoped<IQuartzBaseService, QuartzBaseService>();
            return services;
        }
        public static IServiceCollection AddQuartzServer(this IServiceCollection services)
        {
            services.AddQuartzServer(p => p.WaitForJobsToComplete = true);
            return services;
        }
        private static Action<IServiceCollectionQuartzConfigurator> GetProperty(
            QuartzDBTypeEnum dbType,
            string connectionString,
            string baseSchema,
            string quartPrefix = "",
            string quartzDataSource = "",
            string scheduleId = "",
            string scheduleName = ""
            )
        {
            var datasource = quartzDataSource?? QuartzConstant.DEFAULT_QUARTZ_DATA_SOURCE;
            var frefix = string.IsNullOrEmpty(quartPrefix) ? QuartzConstant.DEFAULT_QUARTZ_FREFIX : quartPrefix;

            var requiredParams = new List<string>();
            if (string.IsNullOrEmpty(connectionString))
                requiredParams.Add("CONNECTION_STRING");
            if (string.IsNullOrEmpty(baseSchema))
                requiredParams.Add("BASE_SCHEMA");

            if (requiredParams.Any())
            {
                throw new Exception($"Param is required. {string.Join(", ", requiredParams)}");
            }

            return p =>
            {
                if (string.IsNullOrEmpty(scheduleId) == false)
                    p.SchedulerId = scheduleId;
                if (string.IsNullOrEmpty(scheduleName) == false)
                    p.SchedulerName = scheduleName;

                p.SetProperty("quartz.serializer.type", "json");
                p.SetProperty("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
                p.SetProperty("quartz.jobStore.dataSource", datasource);
                p.SetProperty("quartz.jobStore.useProperties", "true");
                p.SetProperty("quartz.jobStore.clustered", "true");
                p.SetProperty("quartz.plugin.timezoneConverter.type",
                   "Quartz.Plugin.TimeZoneConverter.TimeZoneConverterPlugin, Quartz.Plugins.TimeZoneConverter");
                switch (dbType)
                {
                    case QuartzDBTypeEnum.POSTGRE:
                        p.SetProperty("quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.PostgreSQLDelegate, Quartz");
                        p.SetProperty("quartz.jobStore.tablePrefix", $"\"{baseSchema}\".{frefix}");
                        p.SetProperty($"quartz.dataSource.{datasource}.provider", "Npgsql");
                        break;
                    case QuartzDBTypeEnum.ORACLE:
                        p.SetProperty("quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.OracleDelegate, Quartz");
                        p.SetProperty("quartz.jobStore.tablePrefix", $"{quartPrefix}");
                        p.SetProperty($"quartz.dataSource.{datasource}.provider", "OracleODPManaged");
                        break;
                    case QuartzDBTypeEnum.SQL_SERVER:
                        p.SetProperty("quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz");
                        p.SetProperty("quartz.jobStore.tablePrefix", $"[{baseSchema}].{frefix}");
                        p.SetProperty($"quartz.dataSource.{datasource}.provider", "SqlServer");
                        break;
                    default:
                        break;
                }
                p.SetProperty($"quartz.dataSource.{datasource}.connectionString", connectionString);

            };
        }
    }
}