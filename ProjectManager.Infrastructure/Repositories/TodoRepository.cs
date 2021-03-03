using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Infrastructure.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<List<Todo>> GetTasks()
        {
            return Entities.ToListAsync();
        }
    }
}