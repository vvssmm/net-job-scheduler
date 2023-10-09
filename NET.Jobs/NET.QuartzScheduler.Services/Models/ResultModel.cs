namespace NET.QuartzScheduler.Services.Models
{
    public class ResultModel<T> : BaseResult
    {
        public T Data { get; set; }
    }
    public class BaseResult
    {
        public bool IsSuccess { get; set; }
        public List<string> StackTraces { get; set; }
        public List<string> Messages { get; set; }

        public BaseResult()
        {
            Messages = new List<string>();
            StackTraces = new List<string>();
            IsSuccess = false;
        }
    }
}
