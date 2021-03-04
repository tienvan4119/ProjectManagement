using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ProjectManager.Domain.Base;

namespace ProjectManager.Domain.Entities
{
    [Table("Milestones")]
    public class Milestone : AuditEntity<string>
    {
        [Required(ErrorMessage = "Title can not be null")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Start Date must be filled")]
        public DateTime StartDate { get; set; }
        public DateTime? EndTime { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Milestone must be added for a certain project")]
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
    }
}
