using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Base.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity); 
        Task<List<T>> GetAll();
        void Attach(T entity);
    }
}
