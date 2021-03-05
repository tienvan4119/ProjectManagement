using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
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
        public async Task<List<Project>> GetProjects()
        {
            var currentUserId = User.Claims.First(_ => _.Type == "UserId").Value;
            var projects = await _projectService.GetProjects();

            return User.IsInRole("Member") ? projects.Where(_ => _.Users.Any(u => u.Id.Equals(currentUserId))).ToList() : projects;
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
