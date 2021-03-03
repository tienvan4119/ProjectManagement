using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Services;
using ProjectManager.Domain.Authentication;
using ProjectManager.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly UserManager<User> _userManager;
        public ClientController(ClientService clientService, UserManager<User> userManager)
        {
            _clientService = clientService;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            client.CreatedBy = User.Claims.First(_ => _.Type == "UserId").Value;
            client.Id = Guid.NewGuid().ToString();
            var result = await _clientService.InsertClient(client);
            return result == 0 ? StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create new Client" }) : Ok(new Response { Status = "Success", Message = "Created new Client successfully" });
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetClients()
        {
            return await _clientService.GetClients();
        }
    }
}
