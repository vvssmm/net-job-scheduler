using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.QuartzScheduler.DataTransfer.Entities
{
    [Table(DatabaseConst.Table.Jobs.TableName)]
    public class JobsEntity : BaseEntity
    {
        [Column(DatabaseConst.Table.Jobs.Cols.Name, TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Name { get; set; }
        [Column(DatabaseConst.Table.Jobs.Cols.JobClass)]
        [MaxLength(250)]
        public string JobClass { get; set; }
        [Column(DatabaseConst.Table.Jobs.Cols.Description, TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
