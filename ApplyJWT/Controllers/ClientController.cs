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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]s")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly UserManager<User> _userManager;
        private readonly ProjectService _projectService;
        public ClientController(ClientService clientService, UserManager<User> userManager, ProjectService projectService)
        {
            _clientService = clientService;
            _userManager = userManager;
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            client.CreatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            client.Id = Guid.NewGuid().ToString();
            var result = await _clientService.InsertClient(client);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create new Client" }) : Ok(new Response { Status = "Success", Message = "Created new Client successfully" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            var client = await _clientService.GetClientById(id);
            if (client == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response
                {
                    Status = "Error",
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
        public async Task<List<Project>> GetProjects(string id)
        {
            var projects = await _projectService.GetProjects();
            return projects.Where(_ => _.ClientId.Equals(id)).ToList();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteClient(string id)
        {
            try
            {
                await _clientService.DeleteClient(id);
                return Ok(new Response {Status = "Success", Message = "Updated Client successfully"});
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
