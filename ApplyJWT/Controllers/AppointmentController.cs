using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Appointment;

namespace ProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        private readonly ProjectService _projectService;
        public AppointmentController(AppointmentService appointmentService, ProjectService projectService)
        {
            appointmentService = _appointmentService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task GetAppointment()
        {

        }

        [HttpGet("{appointmentId}")]
        public async Task GetAppointmentDetail(string appointmentId)
        {

        }

        [HttpPost]
        public async Task AddAppointment([FromBody] AppointmentAddingModel model)
        {

        }

        [HttpPut("{appointmentId}")]
        public async Task UpdateAppointment([FromBody] AppointmentEditingModel model)
        {

        }
    }
}
