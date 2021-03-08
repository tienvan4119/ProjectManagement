using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Base.Repositories;
using ProjectManager.Infrastructure.Interfaces;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<Project>> GetProjects(string status)
        {
            return await Entities.Include(_ => _.Users)
                .Include(_ => _.Tasks)
                .Include(_ => _.Milestones)
                .Include(_ => _.Users)
                .Where(_ => _.Status == (int) Project.Statuses.Close).ToListAsync();
        }

        public Task<Project> GetProjectById(string projectId)
        {
            Entities.Include(_ => _.Users).ToListAsync();
            return Entities.FirstAsync(_ => _.Id.Equals(projectId));
        }
    }
}
