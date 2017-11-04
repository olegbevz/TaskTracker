using System.Collections.Generic;
using System.Threading.Tasks;
using TaskModel = TaskTracker.Domain.Task;
using TaskStatus = TaskTracker.Domain.TaskStatus;

namespace TaskTracker.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetTasks(
            TaskStatus? status = null,
            SortOrder? sortColumn = null,
            int? skip = null,
            int? take = null);

        Task<TaskModel> GetTask(int taskId);
        Task AddTask(TaskModel task);
        Task RemoveTask(int taskId);
        Task UpdateTask(TaskModel task);
    }
}
