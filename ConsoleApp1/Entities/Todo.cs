using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProjectManager.Domain.Base;

namespace ProjectManager.Domain.Entities
{
    public class Todo  :AuditEntity<short>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AttachFile { get; set; }
        public int AssignTo { get; set; }
    }
}
