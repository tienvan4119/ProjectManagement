using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.ViewModels.Milestone
{
    public class EditMilestoneModel
    {
        public DateTime? EndTime { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
    }
}
