using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.ViewModels.Project
{
    public class ProjectAddingModel : ProjectEditingModel
    {
        public string ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Domain.Entities.Client Client { get; set; }
        public virtual ICollection<Todo> Tasks { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Domain.Entities.Milestone> Milestones { get; set; }
    }
}
