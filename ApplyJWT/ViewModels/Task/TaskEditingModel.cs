using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain;

namespace ProjectManager.API.ViewModels.Task
{
    public class TaskEditingModel : AuditEntity<string>
    {
        public enum Statuses
        {
            NotStarted,
            InProgress,
            Completed,
            Closed
        }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string AttachFile { get; set; }
        public string AssignTo { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
