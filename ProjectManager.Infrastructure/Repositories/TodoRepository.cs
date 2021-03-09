using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<List<Todo>> GetAllTasks(string projectId)
        {
            return Entities.Where(_ => _.ProjectId.Equals(projectId)).ToListAsync();
        }

        public Task<List<Todo>> GetTasks(Todo.Statuses result, string projectId)
        {
            return Entities.Where(_ => _.Status == (int)result && _.ProjectId.Equals(projectId)).ToListAsync();
        }

        public Task<Todo> GetTaskById(string taskId)
        {
            return Entities.FirstAsync(_=>_.Id.Equals(taskId));
        }
    }
}