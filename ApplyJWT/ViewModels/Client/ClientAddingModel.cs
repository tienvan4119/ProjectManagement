using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.ViewModels.Client
{
    public class ClientAddingModel : ClientEditingModel
    {
        public virtual ICollection<Domain.Entities.Project> Projects { get; set; }
    }
}
