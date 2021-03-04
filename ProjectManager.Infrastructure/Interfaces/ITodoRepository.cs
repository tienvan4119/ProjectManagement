using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Interface;

namespace ProjectManager.Infrastructure.Interface
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<List<Todo>> GetTasks();
    }
}