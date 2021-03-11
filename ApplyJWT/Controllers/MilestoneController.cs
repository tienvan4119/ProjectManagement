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
    [Route("api/milestones")]
    [ApiController]
    public class MilestoneController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly MilestoneService _milestoneService;
        public MilestoneController(UserManager<User> userManager
            , MilestoneService milestoneService)
        {
            _userManager = userManager;
            _milestoneService = milestoneService;
        }

        [HttpPost]
        public async Task<ActionResult> AddMilestone([FromBody] MilestoneAddingModel model)
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

        [HttpPost("{milestoneId}/members/{memberId}")]
        public async Task<ActionResult> AssignMilestone(string milestoneId, string memberId)
        {
            if (_userManager.FindByIdAsync(memberId) == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Failed when find User to assign"
                });
            }

            var result = await _milestoneService.AssignMilestone(milestoneId, memberId);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Milestone>> GetMilestoneById(string id)
        {
            var milestone = await _milestoneService.GetMilestoneById(id);
            return (ActionResult<Milestone>)milestone ?? StatusCode(StatusCodes.Status200OK, new Response
            {
                Status = "Error 404",
                Message = "Failed to get Milestone detail"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMilestone(string id)
        {
            var milestone = await _milestoneService.GetMilestoneById(id);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMilestone(string id, [FromBody] MilestoneEditingModel model)
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
