using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Document;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;
        public DocumentController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("{projectId}")]
        public async Task GetDocuments(string projectId)
        { 
            //Get Project
            //Get Documents
        }

        [HttpPost("{projectId}")]
        public async Task AddDocument([FromBody] DocumentAddingModel projectId)
        {
            //Get Project
            //Add Document
        }

        [HttpDelete("{documentId}")]
        public async Task DeleteDocument(string documentId)
        {
            //Get Document
            //Delete Document
        }
    }
}
