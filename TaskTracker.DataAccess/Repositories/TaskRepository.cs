using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskModel = TaskTracker.Domain.Task;
using TaskStatus = TaskTracker.Domain.TaskStatus;

namespace TaskTracker.DataAccess.Interfaces
{
    public class TaskRepository : ITaskRepository, IDisposable
    {
        private readonly TaskTrackerContext context;

        public TaskRepository(TaskTrackerContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TaskModel>> GetTasks(TaskStatus? status, SortOrder? sortColumn, int? skip, int? take)
        {
            IQueryable<TaskModel> query = BuildTaskQuery(context.Tasks, status, sortColumn, skip, take);

            return await query.ToArrayAsync();
        }

        public async Task<TaskModel> GetTask(int taskId)
        {
            return await this.context.Tasks.FirstOrDefaultAsync(task => task.Id == taskId);
        }

        public async Task AddTask(TaskModel task)
        {
            task.Added = task.Edited = DateTime.Now;
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTask(int id)
        {
            var task = await context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException();

            context.Tasks.Remove(task);

            await context.SaveChangesAsync();
        }

        public async Task UpdateTask(TaskModel task)
        {            
            var originalTask = await context.Tasks.FindAsync(task.Id);
            if (originalTask == null)
                throw new KeyNotFoundException();

            task.Edited = DateTime.Now;

            context.Entry(originalTask).CurrentValues.SetValues(task);

            await context.SaveChangesAsync();
        }

        private IQueryable<TaskModel> BuildTaskQuery(
            DbSet<TaskModel> tasks, 
            TaskStatus? status, 
            SortOrder? sortColumn, 
            int? skip, 
            int? take)
        {
            IQueryable<TaskModel> query = tasks;

            if (status != null)
                query = query.Where(task => task.Status == status);           

            if (sortColumn != null)
            {
                switch (sortColumn.Value)
                {
                    case SortOrder.Name:
                        query = query.OrderBy(task => task.Name);
                        break;
                    case SortOrder.Priority:
                        query = query.OrderBy(task => task.Priority);
                        break;
                    case SortOrder.Added:
                        query = query.OrderBy(task => task.Added);
                        break;
                    case SortOrder.Duration:
                        query = query.OrderBy(task => task.Duration);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            if (skip != null)
                query = query.Skip(skip.Value);

            if (take != null)
                query = query.Take(take.Value);

            return query;
        }

        public void Dispose()
        {
            // Dispose() is called by ASP.NET Core IoC
            this.context.Dispose();
        }
    }

}
