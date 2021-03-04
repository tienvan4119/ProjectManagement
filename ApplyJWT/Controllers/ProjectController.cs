using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager, Member")]
    [Route("api/[controller]s")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly UserManager<User> _userManager;

        public ProjectController(ProjectService projectService, UserManager<User> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }

        #region Project

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> AddProject([FromBody] Project project)
        {
            project.CreatedBy = User.Claims.First(_ => _.Type.Equals("UserId")).Value;
            var result = await _projectService.InsertProject(project);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create new Project" }) : Ok(new Response { Status = "Success", Message = "Created new project successfully" });
        }

        [HttpGet]
        public async Task<List<Project>> GetProjects()
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            var projects = await _projectService.GetProjects();

            return User.IsInRole("Member") ? projects.Where(_ => _.Users.Any(u => u.Id.Equals(currentUserId))).ToList() : projects;
        }
        #endregion

        #region Task

        [HttpGet]
        public async Task<List<Todo>> GetTasks()
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            var tasks = await _projectService.GetTasks();
            return User.IsInRole("User") ? tasks.Where(_ => _.CreatedBy != null && (_.AssignTo.Equals(currentUserId) || _.CreatedBy.Equals(currentUserId))).ToList() : tasks;
        }

        #endregion

    }
}
