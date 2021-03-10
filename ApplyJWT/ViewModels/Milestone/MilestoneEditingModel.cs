using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.ViewModels.Milestone
{
    public class MilestoneEditingModel
    {
        [Required(ErrorMessage = "Title can not be null")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Start Date must be filled")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Milestone must be added for a certain project")]
        public DateTime? EndTime { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
    }
}
