using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NET.QuartzScheduler.DataAccess
{
    public static class DALServiceExtension
    {
        public static void DALServiceInjection(this IServiceCollection services, string connection)
        {
            services.AddDbContext<QuartzDbContext>(option => option.UseSqlServer(connection));
        }
    }
}
