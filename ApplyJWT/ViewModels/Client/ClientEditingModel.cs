using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain;

namespace ProjectManager.API.ViewModels.Client
{
    public class ClientEditingModel : AuditEntity<string>
    {
        [Required(ErrorMessage = "Client's Name can not be null")]
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
