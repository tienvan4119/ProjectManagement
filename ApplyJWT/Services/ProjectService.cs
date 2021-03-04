using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
            project.Id = Guid.NewGuid().ToString();
            project.CreatedDate = DateTime.Now;
            project.UpdatedDate = DateTime.Now;
            _projectRepository.Add(project);
            return _unitOfWork.SaveChanges();
        }

        public Task<List<Project>> GetProjects()
        {
            return _projectRepository.GetProjects();
        }

        public Task<List<Todo>> GetTasks()
        {
            return _todoRepository.GetTasks();
        }
    }
}
