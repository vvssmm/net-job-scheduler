using NET.QuartzScheduler.DTO.Dtos;

namespace NET.QuartzScheduler.Bus.Services
{
    public interface IJobHistoryService
    {
        Task<bool> InsertLog(JobHistoryDto input);
    }
}
