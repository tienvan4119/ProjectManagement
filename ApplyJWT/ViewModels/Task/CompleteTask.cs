using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.ViewModels.Task
{
    public class CompleteTask
    {
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
    }
}
