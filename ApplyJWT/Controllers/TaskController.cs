using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
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

        [HttpGet]
        public async Task<List<Todo>> GetTasks()
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            var tasks = await _taskService.GetTasks();
            return User.IsInRole("User") ? tasks.Where(_ => _.CreatedBy != null && (_.AssignTo.Equals(currentUserId) || _.CreatedBy.Equals(currentUserId))).ToList() : tasks;
        }

        [HttpPost("{projectId}")]
        public async Task<ActionResult> AddTask(string projectId, [FromBody] Todo task)
        {
            task.CreatedBy = User.Claims.First(_ => _.Type.Equals("UserId")).Value;
            var result = await _taskService.InsertTask(task, projectId);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create Task" }) : Ok(new Response { Status = "Success", Message = "Created new task successfully" });
        }
    }
}
