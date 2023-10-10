using Microsoft.EntityFrameworkCore;
using NET.QuartzScheduler.DataTransfer.Entities;

namespace NET.QuartzScheduler.DataAccess
{
    public class QuartzDbContext:DbContext
    {
        public QuartzDbContext(DbContextOptions<QuartzDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<JobsEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            builder.Entity<JobHistoryEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            builder.Entity<JobSchedulerEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            builder.Entity<SchedulerEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(builder);
        }
        public DbSet<JobsEntity> Jobs { get; set; }
        public DbSet<JobHistoryEntity> JobHistory { get; set; }

        public DbSet<JobSchedulerEntity> JobScheduler { get; set; }

        public DbSet<SchedulerEntity> Scheduler { get; set; }


    }
}