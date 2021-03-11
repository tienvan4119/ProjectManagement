using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using ProjectManager.API.Services;
using ProjectManager.API.ViewModels.Document;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;
        private readonly ProjectService _projectService;
        public DocumentController(DocumentService documentService
            , ProjectService projectService)
        {
            _documentService = documentService;
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<ActionResult> AddDocument([FromBody] DocumentAddingModel model)
        {
            //Get Project
            //Add Document
            var project = _projectService.GetProjectById(model.ProjectId);
            if (project == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Can not find this project"
                });
            model.CreatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            var result = await _documentService.AddDocument(model);
            return result > 0 ? Ok("Created document successfully") : StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "Error",
                Message = "Failed to create document"
            });
        }

        [HttpDelete("{id}")]
        public async Task DeleteDocument(string id)
        {
            //Get Document
            //Delete Document
            var result = await _documentService.DeleteDocument(id);
        }
    }
}
