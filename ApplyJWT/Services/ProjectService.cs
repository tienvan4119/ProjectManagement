using System;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.API.Services
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly ITodoRepository _todoRepository;
        private readonly IClientRepository _clientRepository;

        public ProjectService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , ITodoRepository todoRepository
            , IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _todoRepository = todoRepository;
            _clientRepository = clientRepository;
        }

        public Task<int> InsertProject(Project project)
        {
            _projectRepository.Add(project);
            return _unitOfWork.SaveChanges();
        }
    }
}
