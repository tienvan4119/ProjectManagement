using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.API.Services
{
    public class TaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly ITodoRepository _todoRepository;
        public TaskService(IUnitOfWork unitOfWork
            , IProjectRepository projectRepository
            , ITodoRepository todoRepository)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _todoRepository = todoRepository;
        }
        public Task<List<Todo>> GetTasks()
        {
            return _todoRepository.GetTasks();
        }

        public Task<int> InsertTask(Todo task, string projectId)
        {
            task.Id = Guid.NewGuid().ToString();
            task.Status = 0;
            task.ProjectId = projectId;
            task.CreatedDate = DateTime.Now;
            task.IsDeleted = false;
            _todoRepository.Add(task);
            return _unitOfWork.SaveChanges();
        }
    }
}
