using System.Collections.Generic;

namespace ProjectManager.Infrastructure.Base.Interface
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity); 
        List<T> GetAll();
    }
}
