using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using ProjectManager.API.Services;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]s")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;
        
        private readonly ProjectService _projectService;
        public ClientController(ClientService clientService, ProjectService projectService)
        {
            _clientService = clientService;
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            client.CreatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            
            var result = await _clientService.InsertClient(client);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create new Client" }) : Ok(new Response { Status = "Success", Message = "Created new Client successfully" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            var client = await _clientService.GetClientById(id);
            if (client == null)
            {
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Error 404",
                    Message = "Client not found"
                });
            }
            return client;
        }

        [HttpGet]
        public async Task<List<Client>> GetClients()
        {
            return await _clientService.GetClients();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClient(string id, [FromBody] Client client)
        {
            try
            {
                var userId = User.Claims.First(_ => _.Type == "UserId").Value;
                await _clientService.UpdateClient(id, client, userId);
                return Ok(new Response { Status = "Success", Message = "Updated Client successfully" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response
                {
                    Status = "Error",
                    Message = "Some error happened with the system"
                });
            }
        }

        [HttpGet("GetProject/{id}")]
        public async Task<ActionResult<List<Project>>> GetProjects(string id)
        {
            try
            {
                var projects = await _projectService.GetProjectByClient(id);
                return projects.Count > 0
                    ? Ok(projects)
                    : StatusCode(StatusCodes.Status200OK, new Response
                    {
                        Status = "Not Found",
                        Message = "This Client is not related any Project"
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(string id)
        {
            try
            {
                return await _clientService.DeleteClient(id) != 0 ? StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Deleted Client successfully" }) : StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Can not find this Client" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response
                {
                    Status = "Error",
                    Message = "Some error happened with the system"
                });
            }
        }
    }
}
