﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Infrastructure.Base.Interfaces;

namespace ProjectManager.Infrastructure.Base.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbFactory _dbFactory;
        private DbSet<T> _dbSet;
        public Repository(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;   
        }
        public DbSet<T> Entities => _dbSet ??= _dbFactory.DbContext.Set<T>();

        public void Add(T entity)
        {
            Entities.Add(entity);
            //_dbFactory.DbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            Entities.Remove(entity);
        }

        //public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        //{
        //    return Entities.Where(expression);
        //}

        public void Update(T entity)
        {
            Entities.Update(entity);
            //_dbFactory.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public Task<List<T>> GetAll()
        {
            return Entities.ToListAsync();
        }

        public void Attach(T entity)
        {
            Entities.Attach(entity);
        }
    }
}
