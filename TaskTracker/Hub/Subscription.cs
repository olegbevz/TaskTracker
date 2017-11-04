using TaskTracker.DataAccess.Interfaces;
using TaskStatus = TaskTracker.Domain.TaskStatus;

namespace TaskTracker
{
    public class Subscription
    {
        public TaskStatus? TaskStatus { get; set; }
        public SortOrder? SortOrder { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }

}
