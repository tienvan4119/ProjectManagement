using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.API.ViewModels.Task
{
    public class AddTaskModel : EditTaskModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        public string ProjectId { get; set; }
    }
}
