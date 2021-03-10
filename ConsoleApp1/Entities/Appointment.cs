using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManager.Domain.Entities
{
    [Table("Appointments")]
    public class Appointment : AuditEntity<string>
    {
        [Required(ErrorMessage = "Title can not be null")]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Start date can not be null")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
    }
}
