using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.ViewModels.Document
{
    public class DocumentAddingModel : DocumentEditingModel
    {
        [Required(ErrorMessage = "Must have ProjectId")]
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Domain.Entities.Project Project { get; set; }
    }
}
