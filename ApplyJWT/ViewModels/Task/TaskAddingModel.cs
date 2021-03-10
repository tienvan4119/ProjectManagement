using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.API.ViewModels.Task
{
    public class TaskAddingModel : TaskEditingModel
    {
        [Required(ErrorMessage = "ProjectId is required")]
        public string ProjectId { get; set; }
    }
}
