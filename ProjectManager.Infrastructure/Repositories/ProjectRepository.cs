using System;
using System.Collections.Generic;
using System.Text;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
