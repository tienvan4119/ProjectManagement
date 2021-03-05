using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Interfaces
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<List<Todo>> GetTasks();
    }
}