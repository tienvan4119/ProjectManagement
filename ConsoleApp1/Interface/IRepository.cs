using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ProjectManager.Domain.Interface
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity); 
        List<T> GetAll();
    }
}
