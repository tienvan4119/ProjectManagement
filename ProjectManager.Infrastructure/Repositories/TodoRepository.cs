using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Infrastructure.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}