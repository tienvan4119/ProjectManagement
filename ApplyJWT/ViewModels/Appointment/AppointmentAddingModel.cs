using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.ViewModels.Appointment
{
    public class AppointmentAddingModel : AppointmentEditingModel
    {
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Domain.Entities.Project Project { get; set; }
    }
}
