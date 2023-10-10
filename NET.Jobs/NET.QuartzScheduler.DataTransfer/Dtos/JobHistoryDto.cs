namespace NET.QuartzScheduler.DTO.Dtos
{
    public class JobHistoryDto
    {
        public int JobScheduleId { get; set; }
        public int Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? StartRunningDateTime { get; set; }
        public DateTime? EndRunningDateTime { get; set; }
    }
}
