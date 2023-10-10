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

        public override int SaveChanges()
        {
            var entries = ChangeTracker
        .Entries()
        .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

    }
}