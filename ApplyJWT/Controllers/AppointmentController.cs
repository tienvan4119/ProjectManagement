using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Appointment;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        private readonly ProjectService _projectService;
        public AppointmentController(AppointmentService appointmentService, ProjectService projectService)
        {
            _appointmentService = appointmentService;
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentDetail(string id)
        {
            var appointment = await _appointmentService.GetById(id);
            return appointment != null
                ? Ok(appointment)
                : StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Can not get this appointment's detail"
                });
        }

        [HttpPost]
        public async Task<ActionResult> AddAppointment([FromBody] AppointmentAddingModel model, string id)
        {
            model.CreatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            var result = await _appointmentService.AddAppointment(model);
            return result > 0
                ? Ok("Created Appointment successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Failed to create new appointment"
                });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAppointment([FromBody] AppointmentEditingModel model, string id)
        {
            model.UpdatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            var appointment = await _appointmentService.GetById(id);
            if (appointment == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Can not find this appointment"
                });
            }
            var result = await _appointmentService.UpdateAppointment(appointment, model);
            return result > 0
                ? Ok("Updated appointment successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Failed to update this appointment"
                });
        }
    }
}
