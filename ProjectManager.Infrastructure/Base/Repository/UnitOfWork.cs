using System.Threading.Tasks;
using ProjectManager.Infrastructure.Base.Interface;

namespace ProjectManager.Infrastructure.Base.Repository
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
