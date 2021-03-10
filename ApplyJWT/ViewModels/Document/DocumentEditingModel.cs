using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.ViewModels.Document
{
    public class DocumentEditingModel
    {
        [Required(ErrorMessage = "Document must have a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Must have document URL")]
        public string Url { get; set; }
    }
}