using NET.QuartzScheduler.DataAccess;
using NET.QuartzScheduler.DataTransfer.Entities;
using NET.QuartzScheduler.DTO.Dtos;

namespace NET.QuartzScheduler.Bus.Services.ServiceImplement
{
    public class JobHistoryService : IJobHistoryService
    {
        private readonly QuartzDbContext _dbContext;

        public JobHistoryService(QuartzDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> InsertLog(JobHistoryDto input)
        {

            if (input == null) { return false; }

            var entity = new JobHistoryEntity()
            {
                JobScheduleId = input.JobScheduleId,
                Status = input.Status,
                StartRunningDateTime = input.StartRunningDateTime,
                EndRunningDateTime = input.EndRunningDateTime,
                Description = input.Description,
            };

            _ = await _dbContext.JobHistory.AddAsync(entity);
            int saveChangesRs = _dbContext.SaveChanges();

            return await Task.FromResult(saveChangesRs > 0);
        }
    }
}
