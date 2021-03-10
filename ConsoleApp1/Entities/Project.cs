using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectManager.Domain.Authentication;

namespace ProjectManager.Domain.Entities
{
    [Table("Projects")]
    public partial class Project : AuditEntity<string>
    {
        public enum Statuses
        {
            Open = 0,
            Close = 1
        }
        [Required(ErrorMessage = "Project's Name must be filled")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please input Start Date")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
        public string ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }
        public virtual ICollection<Todo> Tasks { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Milestone> Milestones { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
