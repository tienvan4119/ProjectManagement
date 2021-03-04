using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.API.Services;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]s")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] Project project)
        {
            project.CreatedBy = User.Claims.First(_ => _.Type.Equals("UserId")).Value;
            var result= await _projectService.InsertProject(project);
            if (result == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create new Project" });
            return Ok(new Response {Status = "Success", Message = "Created new project successfully"});
        }

        [HttpGet]
        public async Task<List<Project>> GetProjects()
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            var projects = await _projectService.GetProjects();

            return User.IsInRole("User") ? projects.Where(_ => _.Users.Any(u => u.Id.Equals(currentUserId))).ToList() : projects;
        }

        [HttpGet]
        public async Task<List<Todo>> GetTasks()
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            var tasks = await _projectService.GetTasks();
            return User.IsInRole("User") ? tasks.Where(_ => _.CreatedBy != null && (_.AssignTo.Equals(currentUserId) || _.CreatedBy.Equals(currentUserId))).ToList() : tasks;
        }
    }
}
