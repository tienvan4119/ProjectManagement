using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.API.Services
{
    public class ClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly ITodoRepository _todoRepository;
        private readonly IClientRepository _clientRepository;
        public ClientService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , ITodoRepository todoRepository
            , IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _todoRepository = todoRepository;
            _clientRepository = clientRepository;
        }

        public Task<int> AddClient(Client client)
        {
            client.Id = Guid.NewGuid().ToString();
            client.CreatedDate = DateTime.Now;
            client.UpdatedBy = client.CreatedBy;
            client.UpdatedDate = DateTime.Now;
            client.IsDeleted = false;
            _clientRepository.Add(client);
            return _unitOfWork.SaveChanges();
        }

        public async Task<Client> GetClientById(string id)
        {
            return await _clientRepository.GetClientById(id);
        }

        public async Task<List<Client>> GetClients()
        {
            return await _clientRepository.GetClients();
        }

        public async Task<int> UpdateClient(string id, Client clientUpdate, string updatedBy)
        {
            //client.ApplyTo(clientUpdate);
            // 1. Get client with id =  client.Id

            // 2. Update client info

            // 3. Call Repo update
            var client = GetClientById(id);
            if (clientUpdate.Address == null) { }
            else
            {
                client.Result.Address = clientUpdate.Address;
            }

            if (clientUpdate.ClientName == null) { }

            else
            {
                client.Result.ClientName = clientUpdate.ClientName;
            }

            if (clientUpdate.Phone == null) { }

            else
            {
                client.Result.Phone = clientUpdate.Phone;
            }
            client.Result.UpdatedDate = DateTime.Now;
            client.Result.UpdatedBy = updatedBy;
            _clientRepository.Update(client.Result);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<int> DeleteClient(string id)
        {
            var client = GetClientById(id);
            client.Result.IsDeleted = true;
            _clientRepository.Update(client.Result);
            return await _unitOfWork.SaveChanges();
        }
    }
}
