using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.ViewModels.Milestone
{
    public class AddMilestoneModel : EditMilestoneModel
    {
        [Required(ErrorMessage = "Title can not be null")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Start Date must be filled")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Milestone must be added for a certain project")]
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }

    }
}
