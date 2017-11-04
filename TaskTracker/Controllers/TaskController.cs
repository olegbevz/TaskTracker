using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.DataAccess.Interfaces;
using TaskModel = TaskTracker.Domain.Task;
using TaskStatus = TaskTracker.Domain.TaskStatus;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Http;
using System;

namespace TaskTracker.Controllers
{
    [Route("api/tasks")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository taskRepository;
        private readonly ISubscriptionDictionary subscriptionDictionary;

        public TaskController(ITaskRepository taskRepository, ISubscriptionDictionary subscriptionDictionary)
        {
            this.taskRepository = taskRepository;
            this.subscriptionDictionary = subscriptionDictionary;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(TaskStatus? status, SortOrder? sortOrder, int? skip, int? take)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tasks = await taskRepository.GetTasks(status, sortOrder, skip, take ?? 100);

            return Ok(tasks);
        }

        [HttpGet, Route("task")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await taskRepository.GetTask(taskId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody]TaskModel task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var currentTime = DateTime.UtcNow;
                task.Added = currentTime;
                task.Edited = currentTime;
                task.Status = TaskStatus.Active;

                await taskRepository.AddTask(task);

                UpdateSubscriptions();
            }
            catch (DbUpdateException)
            {
                return InternalError();
            }

            return this.Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTask([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await taskRepository.RemoveTask(id);

                UpdateSubscriptions();

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                return InternalError();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModel task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                task.Edited = DateTime.UtcNow;

                await taskRepository.UpdateTask(task);

                UpdateSubscriptions();

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                return InternalError();
            }
        }

        private void UpdateSubscriptions()
        {
            // TODO: It would be great to determine what subscription we need to update
            foreach (var subscription in subscriptionDictionary.GetAll())
            {
                subscription.PendingUpdate = true;
            }
        }

        private IActionResult InternalError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
