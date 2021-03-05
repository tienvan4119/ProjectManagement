using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Base.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
    }
}
