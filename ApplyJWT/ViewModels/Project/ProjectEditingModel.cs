using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.ViewModels.Project
{
    public class ProjectEditingModel : AuditEntity<string>
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
    }
}
