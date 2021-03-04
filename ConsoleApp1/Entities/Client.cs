using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Domain.Entities
{
    [Table("Clients")]
    public partial class Client : AuditEntity<string>
    {
        [Required(ErrorMessage = "Client's Name can not be null")]
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
