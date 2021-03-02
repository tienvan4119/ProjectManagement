using System.Threading.Tasks;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory _dbFactory;
        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public Task<int> SaveChanges()
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
    }
}
