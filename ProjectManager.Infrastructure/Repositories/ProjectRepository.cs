using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public Task<List<Project>> GetProjects()
        {
            Entities.Include(_ => _.Users).ToListAsync();
            Entities.Include(_ => _.Todos).ToListAsync();
            return Entities.ToListAsync();
        }
    }
}
