using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Appointment;
using ProjectManager.API.ViewModels.Project;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager, Member")]
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly UserManager<User> _userManager;
        private readonly MilestoneService _milestoneService;
        private readonly AppointmentService _appointmentService;
        public ProjectController(ProjectService projectService
            , UserManager<User> userManager
            , MilestoneService milestoneService
            , AppointmentService appointmentService)
        {
            _projectService = projectService;
            _userManager = userManager;
            _milestoneService = milestoneService;
            _appointmentService = appointmentService;
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

        [HttpGet("{id}/members")]
        public async Task<ActionResult<User>> GetMembers(string id)
        {
            //1. Check Permission
            //2 If Manager => get all member of project
            //3 If Member
            //3.1 Check if Member is belong to this project
            var project = await _projectService.GetProjectById(id);
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

        

        [HttpGet("{id}/milestones")]
        public async Task<ActionResult<List<Milestone>>> GetProjectMilestones(string id)
        {
            var milestones = await _milestoneService.GetByProjectId(id);
            return milestones.Count == 0 ? StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Error 404",
                Message = "Can not found this Project"
            }) : milestones;
        }

        

        [Authorize(Roles = "Manager")]
        [HttpPost("{projectId}/members/{memberId}")]
        public async Task<ActionResult> AddMemberToProject(string projectId, string memberId)
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
            var result = await _projectService.AddMemberToProject(project, memberId);
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

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProject(string id, [FromBody] ProjectEditingModel model)
        {
            var project = await _projectService.GetProjectById(id);
            if (project == null)
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Can not find this project"
                });
            model.UpdatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            model.UpdatedDate = DateTime.Now;
            var result = await _projectService.UpdateProject(project, model);
            return result > 0
                ? StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Success",
                    Message = "Updated project successfully"
                })
                : StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Failed to update this project"
                });
        }

        #region Document

        [HttpGet]
        [Route("{id}/documents")]
        public async Task GetDocuments(string id)
        {
            //Get Project
            //Get Documents
        }

        #endregion

        #region Appointment

        [HttpGet("{id}/appointments")]
        public async Task<ActionResult<List<Appointment>>> GetAppointment(string id)
        {
            var appointments = await _appointmentService.GetByProjectId(id);
            return appointments.Count > 0
                ? Ok(appointments)
                : StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "No appointments found"
                });
        }

        #endregion
    }
}