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

namespace ProjectManager.API.Controllers
{
    [Authorize("Admin")]
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
    }
}
