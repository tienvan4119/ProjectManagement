using System;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Authentication;

namespace ProjectManager.Infrastructure
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private readonly Func<ApplicationDbContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ??= _instanceFunc.Invoke();

        public DbFactory(Func<ApplicationDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (_disposed || _dbContext == null) return;
            _disposed = true;
            _dbContext.Dispose();
        }
    }
}
