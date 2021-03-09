using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain;

namespace ProjectManager.API.ViewModels.Task
{
    public class EditTaskModel : AuditEntity<string>
    {
        public enum Statuses
        {
            NotStarted,
            InProgress,
            Completed,
            Closed
        }

        public string Description { get; set; }
        public string AttachFile { get; set; }
        public string AssignTo { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
