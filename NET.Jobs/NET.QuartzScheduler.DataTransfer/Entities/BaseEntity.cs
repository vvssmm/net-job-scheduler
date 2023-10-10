using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.QuartzScheduler.DataTransfer.Entities
{
    public class BaseEntity
    {
        [Key]
        [Column(DatabaseConst.Table.BaseCols.Id)]

        public int Id { get; set; }
        [Column(DatabaseConst.Table.BaseCols.CreatedDate)]

        public DateTime? CreatedDate { get; set; }
        [Column(DatabaseConst.Table.BaseCols.CreatedBy)]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        [Column(DatabaseConst.Table.BaseCols.UpdatedDate)]
        public DateTime? UpdatedDate { get; set; }
        [Column(DatabaseConst.Table.BaseCols.UpdatedBy)]
        [MaxLength(100)]
        public string UpdatedBy { get; set;} = string.Empty;
    }
}
