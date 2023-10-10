using System.ComponentModel.DataAnnotations.Schema;

namespace NET.QuartzScheduler.DataTransfer.Entities
{
    [Table(DatabaseConst.Table.JobScheduler.TableName)]

    public class JobSchedulerEntity : BaseEntity
    {
        [Column(DatabaseConst.Table.JobScheduler.Cols.JobId)]

        public int JobId { get; set; }
        [Column(DatabaseConst.Table.JobScheduler.Cols.ScheduleId)]

        public int ScheduleId { get; set; }
        [Column(DatabaseConst.Table.JobScheduler.Cols.Description)]

        public string Description { get; set; }
        [Column(DatabaseConst.Table.JobScheduler.Cols.Name)]

        public string Name { get; set; } 
    }
}
