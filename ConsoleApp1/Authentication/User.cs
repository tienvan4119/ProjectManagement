using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Authentication
{
    public class User: IdentityUser
    {
        public virtual ICollection<Project> Projects { get; set; }
    }
}
