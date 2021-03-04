using System.Threading.Tasks;

namespace ProjectManager.Domain.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
    }
}
