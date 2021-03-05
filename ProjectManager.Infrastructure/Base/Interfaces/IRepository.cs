using System.Collections.Generic;

namespace ProjectManager.Infrastructure.Base.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity); 
        List<T> GetAll();
        void Attach(T entity);
    }
}
