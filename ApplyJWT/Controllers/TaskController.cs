using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Task;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager, Member")]
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly UserManager<User> _userManager;
        private readonly TaskService _taskService;
        public TaskController(ProjectService projectService, UserManager<User> userManager, TaskService taskService)
        {
            _projectService = projectService;
            _userManager = userManager;
            _taskService = taskService;
        }

        [HttpGet("projects/{id}")]
        public async Task<List<Todo>> GetTasks([FromQuery(Name = "Status")] string status, string id)
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;

            var tasks = await _taskService.GetAllTasks(id);
            return status switch
            {
                "All" => User.IsInRole("Manager")
                    ? tasks
                    : tasks.Where(_ =>
                        {
                            Debug.Assert(_.CreatedBy != null, "_.CreatedBy != null");
                            return _.CreatedBy == currentUserId || _.AssignTo == currentUserId;
                        })
                        .ToList(),
                "Complete" => User.IsInRole("Manager")
                    ? tasks.Where(_ => _.Status == 2 || _.Status == 3).ToList()
                    : tasks.Where(_ =>
                        {
                            Debug.Assert(_.CreatedBy != null, "_.CreatedBy != null");
                            return (_.CreatedBy == currentUserId || _.AssignTo == currentUserId) &&
                                   (_.Status == 2 || _.Status == 3);
                        })
                        .ToList(),
                _ => User.IsInRole("Manager")
                    ? tasks.Where(_ => _.Status == 0 || _.Status == 1).ToList()
                    : tasks.Where(_ =>
                        {
                            Debug.Assert(_.CreatedBy != null, "_.CreatedBy != null");
                            return (_.CreatedBy == currentUserId || _.AssignTo == currentUserId) &&
                                   (_.Status == 0 || _.Status == 1);
                        })
                        .ToList()
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTaskDetail(string taskId)
        {
            var task = await _taskService.GetTaskById(taskId);
            return task != null
                ? Ok(task)
                : StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error",
                    Message = "Can not get detail of this task"
                });
        }

        // Filter Task by Date, User and Project
        [Authorize(Roles = "Manager")]
        [HttpPost("completed")]
        public async Task<ActionResult<List<Todo>>> GetCompleteTask(CompleteTask model)
        {
            var tasks = await _taskService.GetCompleteTask(model);
            return tasks.Count > 0
                ? Ok(tasks)
                : StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Not found",
                    Message = "No data found"
                });
        }

        [HttpPost("projects/{id}")]
        public async Task<ActionResult> AddTask(string id, [FromBody] TaskAddingModel model)
        {
            var task = new Todo
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                AssignTo = model.AssignTo,
                AttachFile = model.AttachFile,
                CreatedBy = User.Claims.First(_ => _.Type == "UserId").Value,
                CreatedDate = DateTime.Now
            };
            var result = await _taskService.InsertTask(task, id);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create Task" }) : Ok(new Response { Status = "Success", Message = "Created new task successfully" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditTask(string id, [FromBody] TaskEditingModel model)
        {
            var task = _taskService.GetTaskById(id);
            task.Result.Status = model.Status;
            var result = await _taskService.EditTask(task.Result);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to Update Task" }) : Ok(new Response { Status = "Success", Message = "Updated task successfully" });
        }

        [HttpPut("{taskId}/members/{memberId}")]
        public async Task<ActionResult> AssignTask(string taskId, string memberId)
        {
            var task = _taskService.GetTaskById(taskId);
            var user = await _userManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Error 404", Message = "Failed to find User" });
            }

            task.Result.AssignTo = memberId;
            var result = await _taskService.EditTask(task.Result);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to Assign Task" }) : Ok(new Response { Status = "Success", Message = "Assigned task successfully" });
        }
    }
}
