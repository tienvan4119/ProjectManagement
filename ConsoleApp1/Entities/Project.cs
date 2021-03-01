#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ProjectManager.Domain.Base;

namespace ProjectManager.Domain.Entities
{
    [Table("Projects")]
    public partial class Project : AuditEntity<short>
    {
        public Project()
        {

        }
        [Required(ErrorMessage = "Project's Name must be filled")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please input Start Date")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
