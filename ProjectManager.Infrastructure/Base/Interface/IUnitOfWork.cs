using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Base.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
    }
}
