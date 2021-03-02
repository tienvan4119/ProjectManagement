using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

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

        public Task<int> InsertClient(Client client)
        {
            _clientRepository.Add(client);
            return _unitOfWork.SaveChanges();
        }
    }
}
