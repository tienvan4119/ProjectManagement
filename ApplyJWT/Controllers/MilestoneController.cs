using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;
using Newtonsoft.Json;
namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MilestoneController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly UserManager<User> _userManager;
        private readonly ProjectService _projectService;
        private readonly MilestoneService _milestoneService;
        public MilestoneController(ClientService clientService, UserManager<User> userManager, ProjectService projectService, MilestoneService milestoneService)
        {
            _clientService = clientService;
            _userManager = userManager;
            _projectService = projectService;
            _milestoneService = milestoneService;
        }

        [HttpGet("GetProjectMilestone/{projectId}")]
        public async Task<ActionResult<List<Milestone>>> GetMilestoneProject(string projectId)
        {
            var project = await _projectService.GetProjectById(projectId);
            if (project == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Can not found this Project"
                });
            }

            return project.Milestones.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddMilestone([FromBody] Milestone milestone)
        {
            milestone.CreatedBy = User.Claims.First(_ => _.Type.Equals("UserId")).Value;
            var result = await _milestoneService.InsertMilestone(milestone);
            if (result == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Failed to create Milestone"
                });
            return StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Success",
                Message = "Created Milestone successfully"
            });
        }

        [HttpPost("AssignTo/{id}")]
        public async Task<ActionResult> AssignMilestone(string id, [FromBody] string assignedUserId)
        {
            if (_userManager.FindByIdAsync(assignedUserId) == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Failed when find User to assign"
                });
            }

            var result = await _milestoneService.AssignMilestone(id, assignedUserId);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "Error",
                Message = "Failed to assign Member to this Milestone"
            }) : StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Success",
                Message = "Assign Milestone to member successfully"
            });
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Milestone>> GetMilestoneById(string id)
        {
            var milestone = await _milestoneService.GetMilestoneById(id);
            return (ActionResult<Milestone>) milestone ?? StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Error 404",
                Message = "Failed to get Milestone detail"
            });
        }
    }
}
