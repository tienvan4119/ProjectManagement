using System.Threading.Tasks;

namespace ProjectManager.Domain.Interface
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}
