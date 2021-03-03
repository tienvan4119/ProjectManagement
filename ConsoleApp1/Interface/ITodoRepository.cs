using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interface
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<List<Todo>> GetTasks();
    }
}