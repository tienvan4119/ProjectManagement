using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<List<Todo>> GetAllTasksOfProject(string projectId);
        Task<List<Todo>> GetTasks(Todo.Statuses result, string projectId);
        Task<Todo> GetTaskById(string taskId);
        Task<List<Todo>> GetCompleteTask();
    }
}