using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using ProjectManager.Domain.Base;

namespace ProjectManager.Domain.Entities
{
    public partial class Todo : AuditEntity<string>
    {
        public enum Statuses
        {
            NotStarted,
            InProgress,
            Completed,
            Closed
        }

        [Required(ErrorMessage = "Name can not be null")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string AttachFile { get; set; }
        public int AssignTo { get; set; }
        [Required(ErrorMessage = "Start date must be set")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Statuses Status { get; set; }
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
    }
}
