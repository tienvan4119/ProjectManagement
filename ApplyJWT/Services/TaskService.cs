using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.ViewModels.Task;
using ProjectManager.Domain.Authentication;
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
        public Task<List<Todo>> GetAllTasks(string projectId)
        {
            return _todoRepository.GetAllTasksOfProject(projectId);
        }

        public async Task<int> InsertTask(Todo task, string projectId)
        {
            task.Status = 0;
            task.ProjectId = projectId;
            task.IsDeleted = false;
            _todoRepository.Add(task);
            return await _unitOfWork.SaveChanges();
        }


        public async Task<Todo> GetTaskById(string taskId)
        {
            return await _todoRepository.GetTaskById(taskId);
        }

        public async Task<int> EditTask(Todo task)
        {
            if (task.Status.Equals(2) || task.Status.Equals(3))
            {
                task.EndDate = DateTime.Now;
            }
            _todoRepository.Update(task);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<List<Todo>> GetCompleteTask(CompleteTask model)
        {
            var tasks = await _todoRepository.GetCompleteTask();
            var exceptTime = DateTime.Parse("01/01/0001");
            return tasks.Where(_ => (_.EndDate.Date == model.EndDate.Date || model.EndDate.Date == exceptTime.Date)
                                    && (_.ProjectId == model.ProjectId || model.ProjectId == null)
                                    && (_.AssignTo == model.UserId || model.UserId == null)).ToList();
        }

        public async Task<List<Todo>> GetCompleteTaskByUser(User user)
        {
            var tasks =  await _todoRepository.GetAll();
            return tasks.Where(_ => _.AssignTo == user.Id && (_.Status == 2 || _.Status == 3)).ToList();
        }
    }
}
