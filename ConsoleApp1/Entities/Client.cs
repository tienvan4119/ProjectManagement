#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ProjectManager.Domain.Base;

namespace ProjectManager.Domain.Entities
{
    [Table("Clients")]
    public partial class Client : AuditEntity<short>
    {
        public Client()
        {

        }
        public string ClientName { get; set; }
        public string? Address { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
