using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.QuartzScheduler.DataTransfer.Entities
{
    [Table(DatabaseConst.Table.JobHistory.TableName)]

    public class JobHistoryEntity : BaseEntity
    {
        [Column(DatabaseConst.Table.JobHistory.Cols.JobScheduleId)]
        public int JobScheduleId { get; set; }
        [Column(DatabaseConst.Table.JobHistory.Cols.Status)]
        public int Status { get; set; }
        [Column(DatabaseConst.Table.JobHistory.Cols.Description, TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [Column(DatabaseConst.Table.JobHistory.Cols.StartRunningDateTime)]

        public DateTime? StartRunningDateTime { get; set; }
        [Column(DatabaseConst.Table.JobHistory.Cols.EndRunningTime)]

        public DateTime? EndRunningDateTime { get; set; }
    }
}
