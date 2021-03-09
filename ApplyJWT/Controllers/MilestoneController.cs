using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Milestone;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]s")]
    [ApiController]
    public class MilestoneController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ProjectService _projectService;
        private readonly MilestoneService _milestoneService;
        public MilestoneController(UserManager<User> userManager
            , ProjectService projectService
            , MilestoneService milestoneService)
        {
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
        public async Task<ActionResult> AddMilestone([FromBody] AddMilestoneModel model)
        {
            var milestone = new Milestone
            {
                Title = model.Title,
                StartDate = model.StartDate,
                EndTime = model.EndTime,
                ProjectId = model.ProjectId,
                AssignedTo = model.AssignedTo,
                Description = model.Description,
                CreatedBy = User.Claims.First(_ => _.Type.Equals("UserId")).Value
            };
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
        public async Task<ActionResult> AssignMilestone(string id, [FromBody] EditMilestoneModel model)
        {
            if (_userManager.FindByIdAsync(model.AssignedTo) == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Failed when find User to assign"
                });
            }

            var result = await _milestoneService.AssignMilestone(id, model.AssignedTo);
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

        [HttpPost("{milestoneId}")]
        public async Task<ActionResult<Milestone>> GetMilestoneById(string milestoneId)
        {
            var milestone = await _milestoneService.GetMilestoneById(milestoneId);
            return (ActionResult<Milestone>)milestone ?? StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Error 404",
                Message = "Failed to get Milestone detail"
            });
        }

        [HttpDelete("{milestoneId}")]
        public async Task<ActionResult> DeleteMilestone(string milestoneId)
        {
            var milestone = await _milestoneService.GetMilestoneById(milestoneId);
            if (milestone == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Can not find this milestone"
                });
            }
            return await _milestoneService.DeleteMilestone(milestone) > 0 ? StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Success",
                Message = "Deleted Milestone successfully"
            }) : StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "Error",
                Message = "Failed to delete this Milestone"
            });
        }

        [HttpPut("AssignTo/{id}")]
        public async Task<ActionResult> UpdateMilestone(string id, [FromBody] EditMilestoneModel model)
        {
            var milestone = await _milestoneService.GetMilestoneById(id);
            if (milestone == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Cannot find Milestone"
                });
            }

            if (model.AssignedTo != null)
            {
                milestone.AssignedTo = model.AssignedTo;
            }

            if (model.Description != null)
            {
                milestone.Description = model.Description;
            }

            if (model.EndTime != null)
            {
                milestone.EndTime = model.EndTime;
            }
            var result = await _milestoneService.UpdateMilestone(milestone);
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
    }
}
