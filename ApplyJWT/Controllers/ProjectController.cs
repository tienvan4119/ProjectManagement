using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> AddProject([FromBody] Project project)
        {
            project.CreatedBy = User.Claims.First(_ => _.Type.Equals("UserId")).Value;
            var result = await _projectService.InsertProject(project);
            return !result.Succeeded ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create new Project" }) : Ok(new Response { Status = "Success", Message = "Created new project successfully" });
        }

        [HttpGet]
        public async Task<List<Project>> GetProjects([FromQuery(Name = "Status")] string filter)
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            if (filter.Equals("All"))
            {
                var projects = await _projectService.GetAllProjects();
                return User.IsInRole("Member") ? projects.Where(_ => _.Users.Any(u => u.Id.Equals(currentUserId))).ToList() : projects;
            }
            else
            {
                var status = filter.Equals("Open") ? Project.Statuses.Open : Project.Statuses.Close;
                var projects = await _projectService.GetProjects(status);
                return User.IsInRole("Member") ? projects.Where(_ => _.Users.Any(u => u.Id.Equals(currentUserId))).ToList() : projects;
            }
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProjectDetail(string projectId)
        {
            var project = await _projectService.GetProjectById(projectId);
            if (project == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Failed to find project's detail"
                });
            }

            if (User.IsInRole("Manager"))
            {
                return project;
            }
            else
            {
                var user = await _userManager.FindByIdAsync(User.Claims.First(_ => _.Type == "UserId").Value);
                return project.Users.Contains(user)
                    ? project
                    : StatusCode(StatusCodes.Status403Forbidden, new Response
                    {
                        Status = "Error",
                        Message = "User do not have permission to view this project's detail"
                    });
            }
        }

        [HttpGet("Members/{projectId}")]
        public async Task<ActionResult<User>> GetMember(string projectId)
        {
            //1. Check Permission
            //2 If Manager => get all member of project
            //3 If Member
            //3.1 Check if Member is belong to this project
            var project = await _projectService.GetProjectById(projectId);
            if (User.IsInRole("Manager"))
            {
                return Ok(project.Users.ToList());
            }
            //Is logged user belong to this project ?
            if (_projectService.IsMemberInProject(project, User.Claims.First(_ => _.Type == "UserId").Value))
            {
                return Ok(project.Users.ToList());
            }
            return StatusCode(StatusCodes.Status403Forbidden, new Response
            {
                Status = "Permission denied",
                Message = "You don't have permission to view this Project's Member"
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("Members/{projectId}")]
        public async Task<ActionResult> AddMemberToProject(string projectId, [FromBody] string userId)
        {
            var project = await _projectService.GetProjectById(projectId);
            if (project == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Project not found"
                });
            }
            var result = await _projectService.AddMemberToProject(project, userId);
            if (result.Succeeded)
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Success",
                    Message = "Add Member to this Project successfully"
                });
            return StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Error 404",
                Message = "Error when add Member to this Project"
            });
        }
    }
}