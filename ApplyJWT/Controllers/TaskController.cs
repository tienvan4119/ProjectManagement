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
    [Route("api/[controller]")]
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

        [HttpGet("{projectId}")]
        public async Task<List<Todo>> GetTasks([FromQuery(Name = "Status")] string status, string projectId)
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;

            var tasks = await _taskService.GetAllTasks(projectId);
            return status switch
            {
                //Enum.TryParse(filter, out Todo.Statuses status);
                //tasks = await _taskService.GetTasks(status, projectId);
                //return User.IsInRole("Member") ? tasks.Where(_ => _.CreatedBy != null && (_.AssignTo == currentUserId || _.CreatedBy.Equals(currentUserId))).ToList() : tasks;
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

        [HttpGet("Detail/{taskId}")]
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
        [HttpGet("Completed")]
        public async Task<ActionResult<List<Todo>>> GetCompleteTask([FromQuery(Name = "Date")] string date)
        {
            var time = DateTime.Parse(date);
            var tasks = await _taskService.GetCompleteTaskByDate(time);
            return tasks.Count > 0
                ? tasks
                : StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "No data",
                    Message = "No task has done today"
                });
        }

        [HttpPost("{projectId}")]
        public async Task<ActionResult> AddTask(string projectId, [FromBody] TaskAddingModel model)
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
            var result = await _taskService.InsertTask(task, projectId);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create Task" }) : Ok(new Response { Status = "Success", Message = "Created new task successfully" });
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult> EditTask(string taskId, [FromBody] TaskEditingModel model)
        {
            var task = _taskService.GetTaskById(taskId);
            task.Result.Status = model.Status;
            var result = await _taskService.EditTask(task.Result);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to Update Task" }) : Ok(new Response { Status = "Success", Message = "Updated task successfully" });
        }

        [HttpPut("AssignTo/{taskId}")]
        public async Task<ActionResult> AssignTask(string taskId, [FromBody] TaskEditingModel model)
        {
            var task = _taskService.GetTaskById(taskId);
            var user = await _userManager.FindByIdAsync(model.AssignTo);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Error 404", Message = "Failed to find User" });
            }
            task.Result.AssignTo = model.AssignTo;
            var result = await _taskService.EditTask(task.Result);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to Assign Task" }) : Ok(new Response { Status = "Success", Message = "Assigned task successfully" });
        }

    }
}
