﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain;

namespace ProjectManager.API.ViewModels.Appointment
{
    public class AppointmentEditingModel : AuditEntity<string>
    {
        [Required(ErrorMessage = "Title can not be null")]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Start date can not be null")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
