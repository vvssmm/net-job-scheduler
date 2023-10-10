using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.QuartzScheduler;
using NET.QuartzScheduler.Services.resources;
using NET.QuartzScheduler.DataAccess;
using NET.QuartzScheduler.Bus.Services;
using NET.QuartzScheduler.Bus.Services.ServiceImplement;

namespace NET.QuartzSheduler.Bus
{
    public static class BusServiceExtension
    {
        public static void BusServiceInjection(this IServiceCollection services, IConfiguration config)
        {
            var connection = config.GetConnectionString("Sql");
            services.RegisterQuartzJDBCStores(
                connectionString: connection, QuartzEnums.QuartzDBTypeEnum.SQL_SERVER, 
                baseSchema:"QRTZ", scheduleId:"SqlScheID", scheduleName: "Sql Schedule Name"
                );

            services.AddScoped<IJobHistoryService, JobHistoryService>();


            services.DALServiceInjection(connection);
        }
    }
}