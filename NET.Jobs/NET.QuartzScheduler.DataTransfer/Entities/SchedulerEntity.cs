using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.QuartzScheduler.DataTransfer.Entities
{
    [Table(DatabaseConst.Table.Scheduler.TableName)]

    public class SchedulerEntity:BaseEntity
    {
        [Column(DatabaseConst.Table.Scheduler.Cols.Name, TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Name { get; set; }
        [Column(DatabaseConst.Table.Scheduler.Cols.CronExpression)]
        [MaxLength(250)]
        public string CronExpression { get; set; }
        [Column(DatabaseConst.Table.Scheduler.Cols.Description, TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
