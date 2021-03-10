using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManager.Domain.Entities
{
    [Table("Documents")]
    public class Document : AuditEntity<string>
    {
        public string ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
        [Required(ErrorMessage = "Document must have a name")]
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
